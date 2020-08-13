using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Obs_Otomasyon
{
    public partial class Anamenü : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Anamenü()
        {
            InitializeComponent();
        }
        universite un;
        private void uni_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (un == null || un.IsDisposed)
            {
                un = new universite();
                un.MdiParent = this;
                un.Show();
            }
        }
        fakulte fk;
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (fk == null || fk.IsDisposed)
            {
                fk = new fakulte();
                fk.MdiParent = this;
                fk.Show();
            }
        }
        bolum bl;
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (bl == null || bl.IsDisposed)
            {
                bl = new bolum();
                bl.MdiParent = this;
                bl.Show();
            }
        }
        ogretimuyesi om;
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (om == null || om.IsDisposed)
            {
                om = new ogretimuyesi();
                om.MdiParent = this;
                om.Show();
            }
        }
        ogrenci ogr;
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ogr == null || ogr.IsDisposed)
            {
                ogr = new ogrenci();
                ogr.MdiParent = this;
                ogr.Show();
            }
        }
        ogrencibilgi ogrb;
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ogrb == null || ogrb.IsDisposed)
            {
                ogrb = new ogrencibilgi();
                ogrb.MdiParent = this;
                ogrb.Show();
            }
        }
        dersac ds;
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ds == null || ds.IsDisposed)
            {
                ds = new dersac();
                ds.MdiParent = this;
                ds.Show();
            }
        }
        dersbilgi db;
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (db == null || db.IsDisposed)
            {
                db = new dersbilgi();
                db.MdiParent = this;
                db.Show();
            }
        }
        dersekle de;
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (de == null || de.IsDisposed)
            {
                de = new dersekle();
                de.MdiParent = this;
                de.Show();
            }
        }
        notgiris ng;
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ng == null || ng.IsDisposed)
            {
                ng = new notgiris();
                ng.MdiParent = this;
                ng.Show();
            }
        }
        personel pr;
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (pr == null || pr.IsDisposed)
            {
                pr = new personel();
                pr.MdiParent = this;
                pr.Show();
            }
        }
        rapor rp;
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (rp == null || rp.IsDisposed)
            {
                rp = new rapor();
                rp.MdiParent = this;
                rp.Show();
            }
        }
    }
}