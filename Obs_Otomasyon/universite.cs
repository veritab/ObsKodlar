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
    public partial class universite : DevExpress.XtraEditors.XtraForm
    {
        public universite()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from universite";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
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
        private void universite_Load(object sender, EventArgs e)
        {
            list();
            sehirler();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"insert into universite(ad,telefon,il,ilce) values (@p1,@p2,@p3,@p4)";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            comm.Parameters.AddWithValue("@p1", txtad.Text);
            comm.Parameters.AddWithValue("@p2", msktel.Text);
            comm.Parameters.AddWithValue("@p3", cmbil.Text);
            comm.Parameters.AddWithValue("@p4", cmbilce.Text);
            comm.ExecuteNonQuery();
            sql.baglanti().Close();
            list();
                MessageBox.Show("Üniversite Kaydı Yapıldı.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"update universite set ad=@p1 ,telefon=@p2 ,il=@p3 ,ilce=@p4 where universiteno=@p5";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", txtad.Text);
                comm.Parameters.AddWithValue("@p2", msktel.Text);
                comm.Parameters.AddWithValue("@p3", cmbil.Text);
                comm.Parameters.AddWithValue("@p4", cmbilce.Text);
                comm.Parameters.AddWithValue("@p5", int.Parse(txtunino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Üniversite Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtunino.Text = dr["universiteno"].ToString();
            txtad.Text = dr["ad"].ToString();
            msktel.Text = dr["telefon"].ToString();
            cmbil.Text = dr["il"].ToString();
            cmbilce.Text = dr["ilce"].ToString();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Üniversiteye Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from bolum where fakulteno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtunino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();

                komut = @"delete from fakulte where unino=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtunino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();

                komut = @"delete from universite where universiteno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtunino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Üniversite Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from universite where ad like '" + txtad.Text+ "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Üniversite Adını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
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