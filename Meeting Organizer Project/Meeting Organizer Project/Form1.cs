using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meeting_Organizer_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void GridDoldur()//DataGrid Dolduran Metod
        {
            using (MeetingOrganizerEntities db = new MeetingOrganizerEntities())
            {
                var sonuc = from t in db.Toplanti
                            select new
                            {
                                t.toplanti_id,
                                t.toplanti_konusu,
                                t.tarih,
                                t.bas_saati,
                                t.bit_saati,
                                t.katilimcilar
                            };
                dataGridView1.DataSource = sonuc.ToList();
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtKonu.Text != "" && dateTarih.Text != "" && txtBasSaati.Text != "" && txtBitSaati.Text != "" && txtKatilimcilar.Text != "")

            {
                
                    using (MeetingOrganizerEntities db = new MeetingOrganizerEntities())
                    {
                        Toplanti t = new Toplanti();
                        t.toplanti_konusu = txtKonu.Text;
                        t.tarih = dateTarih.Value.ToShortDateString();
                        t.bas_saati = txtBasSaati.Text;
                        t.bit_saati = txtBitSaati.Text;
                        t.katilimcilar = txtKatilimcilar.Text;
                        db.Toplanti.Add(t);
                        db.SaveChanges();//veritabanına kaydet
                        MessageBox.Show("Toplantı Kaydedildi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                
                GridDoldur();
            }
            else
            {
                MessageBox.Show("Lütfen Tüm Kayıtları Doldurun", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int tdID = 0;
            if (dataGridView1.SelectedRows.Count > 0)//datagrid seçili ise
            {
                tdID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            }
            using (MeetingOrganizerEntities db = new MeetingOrganizerEntities())
            {
                Toplanti t = db.Toplanti.Find(tdID);
                t.toplanti_konusu = txtKonu.Text;
                t.tarih = dateTarih.Value.ToShortDateString();
                t.bas_saati = txtBasSaati.Text;
                t.bit_saati = txtBitSaati.Text;
                t.katilimcilar = txtKatilimcilar.Text;
                db.SaveChanges();
                MessageBox.Show("Toplantı Güncellendi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (var item in this.Controls)
                {
                    if (item is TextBox)
                    {
                        (item as TextBox).Clear();
                    }
                }
            }
            GridDoldur();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int tdID = 0;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                tdID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                using (MeetingOrganizerEntities db = new MeetingOrganizerEntities())
                {
                    Toplanti t = db.Toplanti.Find(tdID);
                    txtKonu.Text = t.toplanti_konusu;
                    dateTarih.Text = t.tarih;
                    txtBasSaati.Text = t.bas_saati;
                    txtBitSaati.Text = t.bit_saati;
                    txtKatilimcilar.Text = t.katilimcilar;
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show("Silmek İstediğinize Emin Misiniz?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap == DialogResult.Yes)
            {

                int tdID = 0;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    tdID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                }
                using (MeetingOrganizerEntities db = new MeetingOrganizerEntities())
                {
                    Toplanti t = db.Toplanti.Find(tdID);
                    db.Toplanti.Remove(t);
                    db.SaveChanges();
                    MessageBox.Show("Toplantı Silindi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (var item in this.Controls)
                    {
                        if (item is TextBox)
                        {
                            (item as TextBox).Clear();
                        }
                    }
                }
                GridDoldur();
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Clear();
                    
                }
                if (item is MaskedTextBox)
                {
                    (item as MaskedTextBox).Clear();
                }
            }
            //Tüm Textboxlar Temizlendi
        }

        private void txtBasSaati_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); //Toplantı Saati TextBox kısmına sadece Sayı girilmesi sağlandı
        }

        private void txtBitSaati_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);//Toplantı Saati TextBox kısmına sadece Sayı girilmesi sağlandı
        }

    }
}
