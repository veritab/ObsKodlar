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
    public partial class bolum : DevExpress.XtraEditors.XtraForm
    {
        public bolum()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from bolum";
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
        void baskan()
        {
            komut = @"Select sicilno,ad,soyad from ogretimuyesi";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookogretim.Properties.ValueMember = "sicilno";
            lookogretim.Properties.DisplayMember = "ad";
            lookogretim.Properties.DisplayMember = "soyad";
            lookogretim.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void bolum_Load(object sender, EventArgs e)
        {
            list();
            fakulteler();
            baskan();
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


                komut = @"Select sicilno from ogretimuyesi where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogretim.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lblbaskan.Text = dr1["sicilno"].ToString();
                }

                komut = @"insert into bolum(fakulteno,bolumbaskani,bolumadi) values (@p1,@p2,@p3)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblfakulte.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblbaskan.Text));
                comm.Parameters.AddWithValue("@p3", txtad.Text);
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Bölüm Kaydı Yapıldı.");
            }
            catch (Exception)
            {

                MessageBox.Show("Lütfen Tüm Alanları Doldurduğunuzdan Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtbolumno.Text = dr["bolumno"].ToString();
            lookfakulte.Text = dr["fakulteno"].ToString();
            lblfakulte.Text = dr["fakulteno"].ToString();
            lookogretim.Text = dr["bolumbaskani"].ToString();
            lblbaskan.Text = dr["bolumbaskani"].ToString();
            txtad.Text = dr["bolumadi"].ToString();
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


                komut = @"Select sicilno from ogretimuyesi where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogretim.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lblbaskan.Text = dr1["sicilno"].ToString();
                }

                komut = @"update bolum set fakulteno=@p1 ,bolumbaskani=@p2 ,bolumadi=@p3 where bolumno=@p4";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblfakulte.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblbaskan.Text));
                comm.Parameters.AddWithValue("@p3", txtad.Text);
                comm.Parameters.AddWithValue("@p4", int.Parse(txtbolumno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Bölüm Kaydı Güncellendi.");
            }
         catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Bölüm Kaydına Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from bolum where bolumno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtbolumno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğretim Üyesi Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from bolum where bolumadi like '" + txtad.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Bölüm Adı Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}