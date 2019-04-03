using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Academy.Apis.Data;
using Academy.Apis.UI.WinClient.localhost;
using Newtonsoft.Json;

namespace Academy.Apis.UI.WinClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            localhost.UserAdmin _userAdminProxy = new localhost.UserAdmin();

            localhost.UserDO _userInfo = new localhost.UserDO()
            { 
               Name =  this.textBox1.Text,
               LastName = this.textBox2.Text,
               Email = this.textBox3.Text,
               NickName = this.textBox4.Text
            };
            try
            {

                localhost.OperationResultOfInt32 _opResult =

                _userAdminProxy.CreateUser(_userInfo);

                this.textBox5.Text = string.Format("{0} - {1} [{2}]", _opResult.OpStatus, _opResult.OpMessage, _opResult.OpResult );

                MessageBox.Show(this, _opResult.OpMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information); ;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);;
            }
        }


        Academy.Apis.Data.DataObjects.UserDO userObject = null;

        private void showFilecontent(string fileName)
        {
            this.textBox5.Text =  File.ReadAllText(fileName);
        }

        private void showFilecontentB(string fileName)
        {
            //Se seleccionan todos los Bytes del archivo
            byte[] fileBytes = File.ReadAllBytes(fileName);
            StringBuilder sb = new StringBuilder();
            //se crea la cadena con los Bytes leidos
            foreach (byte b in fileBytes)
            {
                //Parametros de interpretacion (Byte, Base 2)
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            this.textBox5.Text = sb.ToString();
        }

        private void tmiSaveXml_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog _fsd = new SaveFileDialog()
                    {
                        AddExtension = true,
                        Filter = "*.xml|*.xml",
                        CheckPathExists = true
                    })
            {

                if (DialogResult.OK.Equals(_fsd.ShowDialog(this)))
                {
                    Academy.Apis.Data.DataObjects.UserDO _userInfo = new Academy.Apis.Data.DataObjects.UserDO()
                    {
                        Name = this.textBox1.Text,
                        LastName = this.textBox2.Text,
                        Email = this.textBox3.Text,
                        NickName = this.textBox4.Text
                    };

                    string filename = _fsd.FileName;
                    XmlSerializer ser = new XmlSerializer(typeof(Academy.Apis.Data.DataObjects.UserDO));
                    using (TextWriter writer = new StreamWriter(filename))
                    {
                        ser.Serialize(writer, _userInfo);
                        writer.Close();
                        showFilecontent(filename);
                    }
                }

            }
            

        }

        private void tmiOpenXml_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog _fod = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = "*.xml|*.xml",
                CheckPathExists = true,
                CheckFileExists = true
            }
        )
            {

                if (DialogResult.OK.Equals(_fod.ShowDialog(this)))
                {
                    Academy.Apis.Data.DataObjects.UserDO _userInfo = null;

                    string filename = _fod.FileName;
                    XmlSerializer ser = new XmlSerializer(typeof(Academy.Apis.Data.DataObjects.UserDO));
                    using (FileStream _streamReader =
                        new FileStream(filename, FileMode.Open))
                    {
                        _userInfo = (Academy.Apis.Data.DataObjects.UserDO)ser.Deserialize(_streamReader);
                        _streamReader.Close();
                    }
                    if (null != _userInfo)
                    {
                        this.textBox1.Text = _userInfo.Name;
                        this.textBox2.Text = _userInfo.LastName;
                        this.textBox3.Text = _userInfo.Email;
                        this.textBox4.Text = _userInfo.NickName;
                    }
                    showFilecontent(filename);
                }

            }

        }

        private void tmiOpenBinary_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog _fod = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = "*.bin|*.bin",
                CheckPathExists = true,
                CheckFileExists = true
            })

            {
                if (DialogResult.OK.Equals(_fod.ShowDialog(this)))
                {
                    string filename = _fod.FileName;
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(filename, FileMode.Open, 
                        FileAccess.Read, FileShare.Read);
                    Academy.Apis.Data.DataObjects.UserDO _userInfo =
                        (Academy.Apis.Data.DataObjects.UserDO)formatter.Deserialize(stream);
                    stream.Close();
                    if (null != _userInfo)
                    {
                        this.textBox1.Text = _userInfo.Name;
                        this.textBox2.Text = _userInfo.LastName;
                        this.textBox3.Text = _userInfo.Email;
                        this.textBox4.Text = _userInfo.NickName;
                    }

                    showFilecontentB(filename);

                }
            }
        }

        private void tmiSaveBinary_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog _fsd = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = "*.bin|*.bin",
                CheckPathExists = true
            })
            {

                if (DialogResult.OK.Equals(_fsd.ShowDialog(this)))
                {
                    Academy.Apis.Data.DataObjects.UserDO _userInfo = new Academy.Apis.Data.DataObjects.UserDO()
                    {
                        Name = this.textBox1.Text,
                        LastName = this.textBox2.Text,
                        Email = this.textBox3.Text,
                        NickName = this.textBox4.Text
                    };

                    string filename = _fsd.FileName;
                    //
                    IFormatter Bformatter = new BinaryFormatter();
                    Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                    try
                    {
                        Bformatter.Serialize(stream, _userInfo);
                        stream.Close();
                        showFilecontentB(filename);
                    }
                    catch (SerializationException ex)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + ex.Message);
                        throw;
                    }
                    finally
                    {
                        stream.Close();
                    }

                    showFilecontentB(filename);


                }

            }
        }

        private void Btn_SqlDirect_Click(object sender, EventArgs e)
        {
            UserAdmin _userAdminProxy = new UserAdmin();
            UserDO _userInfo = new UserDO()
            {
                Name = this.textBox1.Text,
                LastName = this.textBox2.Text,
                Email = this.textBox3.Text,
                NickName = this.textBox4.Text
            };
            _userAdminProxy.CreateUser(_userInfo);

        }

        private void tmiSaveJson_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog _fsd = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = "*.json|*.json",
                CheckPathExists = true
            })
            {

                if (DialogResult.OK.Equals(_fsd.ShowDialog(this)))
                {
                    Academy.Apis.Data.DataObjects.UserDO _userInfo = new Academy.Apis.Data.DataObjects.UserDO()
                    {
                        Name = this.textBox1.Text,
                        LastName = this.textBox2.Text,
                        Email = this.textBox3.Text,
                        NickName = this.textBox4.Text
                    };

                    string filename = _fsd.FileName;
                    //---------------File save text
                    File.WriteAllText(filename, JsonConvert.SerializeObject(_userInfo));

                    // serialize JSON directly to a file
                    using (StreamWriter file = File.CreateText(filename))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, _userInfo);
                    }
                    //-----------------------------------
                    showFilecontent(filename);
                }


            }
        }

        private void tmiOpenJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog _fod = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = "*.json|*.json",
                CheckPathExists = true,
                CheckFileExists = true
            })

            {
                if (DialogResult.OK.Equals(_fod.ShowDialog(this)))
                {
                    string filename = _fod.FileName;
                    /*// read file into a string and deserialize JSON to a type
                    Academy.Apis.Data.DataObjects.UserDO _userInfo = 
                        JsonConvert.DeserializeObject<Academy.Apis.Data.DataObjects.UserDO>
                        (File.ReadAllText(filename));
                        */
                    // deserialize JSON directly from a file
                    using (StreamReader file = File.OpenText(filename))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Academy.Apis.Data.DataObjects.UserDO _userInfo = 
                            (Academy.Apis.Data.DataObjects.UserDO)serializer.Deserialize
                            (file, typeof(Academy.Apis.Data.DataObjects.UserDO));
                    
                    if (null != _userInfo)
                    {
                        this.textBox1.Text = _userInfo.Name;
                        this.textBox2.Text = _userInfo.LastName;
                        this.textBox3.Text = _userInfo.Email;
                        this.textBox4.Text = _userInfo.NickName;
                    }

                    showFilecontent(filename);
                    }
                }
            }

        }
    }
}
