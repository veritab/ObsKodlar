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
    public partial class dersbilgi : DevExpress.XtraEditors.XtraForm
    {
        public dersbilgi()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from dersbilgi";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void ders()
        {
            komut = @"Select dersno from acilanders";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            NpgsqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                cmbdersno.Properties.Items.Add(dr[0]);
            }
            sql.baglanti().Close();
        }
        void bolumler()
        {
            komut = @"Select bolumno,bolumadi from bolum";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookbolum.Properties.ValueMember = "bolumno";
            lookbolum.Properties.DisplayMember = "bolumadi";
            lookbolum.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void dersbilgi_Load(object sender, EventArgs e)
        {
            list();
            ders();
            bolumler();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {

                komut = @"Select bolumno from bolum where bolumadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookbolum.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblbolum.Text = dr["bolumno"].ToString();
                }


                komut = @"insert into dersbilgi(dersno,dersadi,kredi,bolumno) values (@p1,@p2,@p3,@p4)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(cmbdersno.Text));
                comm.Parameters.AddWithValue("@p2", txtdersad.Text);
                comm.Parameters.AddWithValue("@p3", int.Parse(txtkredi.Text));
                comm.Parameters.AddWithValue("@p4", int.Parse(lblbolum.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Bilgi Kaydı Yapıldı.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            cmbdersno.Text = dr["dersno"].ToString();
            txtdersad.Text = dr["dersadi"].ToString();
            txtkredi.Text = dr["kredi"].ToString();
            lookbolum.Text = dr["bolumno"].ToString();
            lblbolum.Text = dr["bolumno"].ToString();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"update dersbilgi set dersadi=@p1 ,kredi=@p2 ,bolumno=@p3 where dersno=@p4";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", txtdersad.Text);
                comm.Parameters.AddWithValue("@p2", int.Parse(txtkredi.Text));
                comm.Parameters.AddWithValue("@p3", int.Parse(lblbolum.Text));
                comm.Parameters.AddWithValue("@p4", int.Parse(cmbdersno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Bilgi Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Ders Bilgi Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from dersbilgi where dersno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(cmbdersno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Bilgi Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from dersbilgi where dersadi like '" + txtdersad.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Ders Adı Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}