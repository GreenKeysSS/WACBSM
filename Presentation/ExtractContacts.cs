using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace Presentation
{
    public partial class ExtractContacts : Form
    {

        private WAButtfrm wabutt;
        public ExtractContacts(WAButtfrm wabutt)
        {
            InitializeComponent();
            this.wabutt = wabutt;

            this.Size = new Size(468, 151);








        }

        private async  void extractbtn_Click(object sender, EventArgs e)
        {
             IWebDriver driver = WA.driver;
 

            if (tosearchtxt.Text!="")
            {
                if (!wabutt.wa.IsBrowserClosed())
                {

                    await wabutt.GetContactsFromGroup(tosearchtxt.Text);

                    if (WAButtfrm.filenameextracted != string.Empty)
                    {
                        extracteddgv.DataSource = WAButtfrm.ReadCSV3(WAButtfrm.filenameextracted);


                        this.Size = new Size(468, 544);
                        extracteddgv.Visible = true;
                    }
                    else
                    {
                      MessageBox.Show("El grupo ingresado no exite en la lista de grupos de WhatsApp", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }






                }
                else
                {
                    
                    DialogResult d;
                    d = MessageBox.Show("El navegador está cerrado, no se pueden obtener los contactos!, conecte otra vez presionando <Conectar WhatsApp>", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    
                    this.Close();

                }
            }
            else
            {
                MessageBox.Show("Completar la casilla", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
           
            
         

            

        }

        private void exportcsvbtn_Click(object sender, EventArgs e)
        {
            string[] words = WAButtfrm.GetWords(tosearchtxt.Text);
            string converted = "";

            foreach (var item in words)
            {
                converted = converted + item;
            }

            WAButtfrm.StoreGroupContacts(converted, WAButtfrm.strex);
        }

        private void tocontactsbtn_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in extracteddgv.Rows)
                {
                    

                      wabutt.contactsdgv.Rows.Add(Convert.ToString(row.Cells[0].Value),"","");

                     

                }

                MessageBox.Show("Agregados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                wabutt.maintab.SelectedTab = wabutt.maintab.TabPages[0];
                
                this.Close();
                wabutt.ClearEmptyRows(wabutt.contactsdgv);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }
    }
}
