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
    public partial class ogrencibilgi : DevExpress.XtraEditors.XtraForm
    {
        public ogrencibilgi()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from ogrencibilgi";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void ogrenciler()
        {
            komut = @"Select ogrencino,ad,soyad from ogrenci";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookogrenci.Properties.ValueMember = "ogrencino";
            lookogrenci.Properties.DisplayMember = "ad";
            lookogrenci.Properties.DisplayMember = "soyad";
            lookogrenci.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        void sehirler()
        {
            komut = @"Select ilad from il";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            NpgsqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                cmbil.Properties.Items.Add(dr[0]);
            }
            sql.baglanti().Close();
        }
        private void ogrencibilgi_Load(object sender, EventArgs e)
        {
            list();
            ogrenciler();
            sehirler();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            lookogrenci.Text = dr["ogrencino"].ToString();
            lblogrenci.Text = dr["ogrencino"].ToString();
            cmbil.Text = dr["il"].ToString();
            cmbilce.Text = dr["ilce"].ToString();
            msktc.Text = dr["tc"].ToString();
            msktel.Text = dr["tel"].ToString();
            txtadres.Text = dr["adres"].ToString();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {

                komut = @"Select ogrencino from ogrenci where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookogrenci.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblogrenci.Text = dr["ogrencino"].ToString();
                }

                komut = @"insert into ogrencibilgi(ogrencino,il,ilce,adres,tc,tel) values (@p1,@p2,@p3,@p4,@p5,@p6)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblogrenci.Text));
                comm.Parameters.AddWithValue("@p2", cmbil.Text);
                comm.Parameters.AddWithValue("@p3", cmbilce.Text);
                comm.Parameters.AddWithValue("@p4", txtadres.Text);
                comm.Parameters.AddWithValue("@p5", msktc.Text);
                comm.Parameters.AddWithValue("@p6", msktel.Text);
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğrenci Bilgisi Kaydı Yapıldı.");
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
                komut = @"update ogrencibilgi set il=@p1 ,ilce=@p2,adres=@p3 ,tc=@p4,tel=@p5 where ogrencino=@p6";
                comm = new NpgsqlCommand(komut, sql.baglanti());               
                comm.Parameters.AddWithValue("@p1", cmbil.Text);
                comm.Parameters.AddWithValue("@p2", cmbilce.Text);
                comm.Parameters.AddWithValue("@p3", txtadres.Text);
                comm.Parameters.AddWithValue("@p4", msktc.Text);
                comm.Parameters.AddWithValue("@p5", msktel.Text);
                comm.Parameters.AddWithValue("@p6", int.Parse(lblogrenci.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğrenci Bilgi Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Öğrenciye Ait Tüm Bilgi Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from ogrencibilgi where ogrencino=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lblogrenci.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Öğrenci Bilgi Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            try
            {
                komut = @"Select * from ogrencibilgi where tc like '" + msktc.Text + "'";
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

        private void cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbilce.Properties.Items.Clear();
            komut = @"select ilcead from ilce where  ilid=@p1";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            comm.Parameters.AddWithValue("@p1", cmbil.SelectedIndex + 1);
            NpgsqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                cmbilce.Properties.Items.Add(dr[0]);
            }
            sql.baglanti().Close();
        }
    }
}