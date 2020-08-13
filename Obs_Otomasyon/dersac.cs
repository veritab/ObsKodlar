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
    public partial class dersac : DevExpress.XtraEditors.XtraForm
    {
        public dersac()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from acilanders";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
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
        void ogretimuyeleri()
        {
            komut = @"Select sicilno,ad,soyad from ogretimuyesi";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookogretim.Properties.ValueMember = "universiteno";
            lookogretim.Properties.DisplayMember = "ad";
            lookogretim.Properties.DisplayMember = "soyad";
            lookogretim.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void dersac_Load(object sender, EventArgs e)
        {
            list();
            bolumler();
            ogretimuyeleri();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtderskodu.Text = dr["derskodu"].ToString();
            lookogretim.Text = dr["ogretimuyesi"].ToString();
            lblogretim.Text = dr["ogretimuyesi"].ToString();
            lookbolum.Text = dr["bolum"].ToString();
            lblbolum.Text = dr["bolum"].ToString();
            txtdersno.Text = dr["dersno"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select sicilno from ogretimuyesi where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogretim.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lblogretim.Text = dr1["sicilno"].ToString();
                }

                komut = @"Select bolumno from bolum where bolumadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookbolum.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblbolum.Text = dr["bolumno"].ToString();
                }

                komut = @"insert into acilanders(ogretimuyesi,bolum) values (@p1,@p2)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblogretim.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblbolum.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Açılma Kaydı Yapıldı.");
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

                komut = @"Select sicilno from ogretimuyesi where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogretim.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lblogretim.Text = dr1["sicilno"].ToString();
                }

                komut = @"Select bolumno from bolum where bolumadi=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookbolum.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblbolum.Text = dr["bolumno"].ToString();
                }

                komut = @"update acilanders set ogretimuyesi=@p1 ,bolum=@p2  where derskodu=@p3";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblogretim.Text));
                comm.Parameters.AddWithValue("@p2", int.Parse(lblbolum.Text));
                comm.Parameters.AddWithValue("@p3", int.Parse(txtderskodu.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Ders Kaydına Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from acilanders where derskodu=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtderskodu.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Ders Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from acilanders where dersno= '" + txtdersno.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Ders No Alanını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}