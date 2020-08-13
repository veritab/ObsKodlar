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
    public partial class fakulte : DevExpress.XtraEditors.XtraForm
    {
        public fakulte()
        {
            InitializeComponent();
        }
        sqlbağlan sql = new sqlbağlan();
        private string komut;
        private NpgsqlCommand comm;
        private DataTable dt;

        void list()
        {
            komut = @"Select * from fakulte";
            comm = new NpgsqlCommand(komut, sql.baglanti());
            dt = new DataTable();
            dt.Load(comm.ExecuteReader());
            sql.baglanti().Close();
            gridControl1.DataSource = dt;
        }
        void universiteler()
        {
            komut = @"Select universiteno,ad  from universite";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookuni.Properties.ValueMember = "universiteno";
            lookuni.Properties.DisplayMember = "ad";
            lookuni.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        void ogretimuyeleri()
        {
            komut = @"Select sicilno,ad,soyad from ogretimuyesi";
            dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
            da.Fill(dt);
            lookdekan.Properties.ValueMember = "universiteno";
            lookdekan.Properties.DisplayMember = "ad";
            lookdekan.Properties.DisplayMember = "soyad";
            lookdekan.Properties.DataSource = dt;
            sql.baglanti().Close();
        }
        private void fakulte_Load(object sender, EventArgs e)
        {
            list();
            universiteler();
            ogretimuyeleri();

        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtfakulteno.Text = dr["fakulteno"].ToString();
            lookuni.Text = dr["unino"].ToString();
            lblunino.Text = dr["unino"].ToString();
            lookdekan.Text = dr["dekan"].ToString();
            lbldekan.Text = dr["dekan"].ToString();
            txtad.Text = dr["fakulteadi"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
          try
            {

                komut = @"Select universiteno from universite where ad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookuni.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblunino.Text = dr["universiteno"].ToString();
                }

                  komut = @"Select sicilno from ogretimuyesi where soyad=@p1";
                  comm = new NpgsqlCommand(komut, sql.baglanti());
                    comm.Parameters.AddWithValue("@p1", lookdekan.Text);
                    NpgsqlDataReader dr1 = comm.ExecuteReader();
                    while (dr1.Read())
                    {
                        lbldekan.Text = dr1["sicilno"].ToString();
                    }
                
                komut = @"insert into fakulte(dekan,fakulteadi,unino) values (@p1,@p2,@p3)";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lbldekan.Text));
                comm.Parameters.AddWithValue("@p2", txtad.Text);
                comm.Parameters.AddWithValue("@p3", int.Parse(lblunino.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Fakulte Kaydı Yapıldı.");
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
                komut = @"Select universiteno from universite where ad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookuni.Text);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    lblunino.Text = dr["universiteno"].ToString();
                }

                komut = @"Select sicilno from ogretimuyesi where soyad=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", lookdekan.Text);
                NpgsqlDataReader dr1 = comm.ExecuteReader();
                while (dr1.Read())
                {
                    lbldekan.Text = dr1["sicilno"].ToString();
                }

                komut = @"update fakulte set dekan=@p1 ,fakulteadi=@p2 ,unino=@p3  where fakulteno=@p4";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(lbldekan.Text));
                comm.Parameters.AddWithValue("@p2", txtad.Text);
                comm.Parameters.AddWithValue("@p3", int.Parse(lblunino.Text));
                comm.Parameters.AddWithValue("@p4", int.Parse(txtfakulteno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Fakulte Kaydı Güncellendi.");
            }
            catch (Exception)
            {
                MessageBox.Show("Eksik veya Hatalı Değer Girdiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eminmisiniz Fakulteye Ait Tüm Kayıtlarınız Silinicek!!!", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                komut = @"delete from universite where universiteno=@p1";
                comm = new NpgsqlCommand(komut, sql.baglanti());
                comm.Parameters.AddWithValue("@p1", int.Parse(txtfakulteno.Text));
                comm.ExecuteNonQuery();
                sql.baglanti().Close();
                list();
                MessageBox.Show("Fakulte Kaydı Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                komut = @"Select * from fakulte where ad like '" + txtad.Text + "'";
                dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut, sql.baglanti());
                da.Fill(dt);
                gridControl1.DataSource = dt;
                sql.baglanti().Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Fakulte Adını Girdiğinizden Emin Olun !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
        }
    }
}