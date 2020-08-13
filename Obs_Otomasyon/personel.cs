using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Npgsql;

namespace Obs_Otomasyon
{
    public partial class personel : DevExpress.XtraEditors.XtraForm
    {
        public personel()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from personel";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void fakulteler()
        {
            komut = @"Select fakulteno,fakulteadi from fakulte";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookfakulte.Properties.ValueMember = "fakulteno";
            lookfakulte.Properties.DisplayMember = "fakulteadi";
            lookfakulte.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void personel_Load(object sender, EventArgs e)
        {
            list();
            fakulteler();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtpersonelno.Text = dr["personelno"].ToString();
            lookfakulte.Text = dr["fakulteno"].ToString();
            lblfakulte.Text = dr["fakulteno"].ToString();
            txtad.Text = dr["ad"].ToString();
            txtsoyad.Text = dr["soyad"].ToString();
            msktc.Text = dr["tc"].ToString();
            txtmevki.Text = dr["mevki"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {


                komut = @"Select fakulteno from fakulte where fakulteadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookfakulte.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblfakulte.Text = dr["fakulteno"].ToString();
                }

                komut = @"insert into personel(ad,soyad,tc,fakulteno,mevki) values (@p1,@p2,@p3,@p4,@p5)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", txtad.Text);
                comm.Parameters.AddWithValue("@p2", txtsoyad.Text);
                comm.Parameters.AddWithValue("@p3", msktc.Text);
                comm.Parameters.AddWithValue("@p4", int.Parse(lblfakulte.Text));
                comm.Parameters.AddWithValue("@p5", txtmevki.Text);
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Personel Kaydı Yapıldı.");
            }
            catch (Exception)
            {

                MessageBox.Show("Lütfen Tüm Alanları Doldurduğunuzdan Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select fakulteno from fakulte where fakulteadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookfakulte.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblfakulte.Text = dr["fakulteno"].ToString();
                }

                komut = @"update personel set tc=@p1 ,ad=@p2 ,soyad=@p3,fakulteno=@p4,mevki=@p5  where personelno=@p6";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", msktc.Text);
                comm.Parameters.AddWithValue("@p2", txtad.Text);
                comm.Parameters.AddWithValue("@p3", txtsoyad.Text);
                comm.Parameters.AddWithValue("@p4", int.Parse(lblfakulte.Text));
                comm.Parameters.AddWithValue("@p5", txtmevki.Text);
                comm.Parameters.AddWithValue("@p6", int.Parse(txtpersonelno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Personel Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Personel'e Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from personel where personelno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtpersonelno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Personel Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from personel where tc like '" + msktc.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Tc Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}