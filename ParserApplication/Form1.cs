using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ParserApplication {
    public partial class Parser:Form {
        public Parser () {
            InitializeComponent();
            txtFile.Enabled = false;
        }

        private void btnFileSelect_Click ( object sender, EventArgs e ) {
            var openFileDialog = new OpenFileDialog() {
                Filter = "Text files (*.y)|*.y",
                Title = "ArchivoSeleccionado"
            };

            try {
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    var filePath = openFileDialog.FileName;
                    var fileName = Path.GetFileName(filePath);
                    DialogResult dialogResult = MessageBox.Show($"¿Desea analizar la gramática que se encuentra en {fileName}?", "Confirmar análisis", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) {
                        txtFile.Text = filePath;
                        txtFile.Enabled = false;
                        grammarAnalyzer(openFileDialog.OpenFile(), fileName);
                    }
                }
            }catch {
                MessageBox.Show("El formato del archivo de entrada es incorrecto, \npor favor inténtelo nuevamente.");
            }
        }

        private void grammarAnalyzer (Stream fileStream, string fileName) {
            using (StreamReader reader = new StreamReader(fileStream)) {
                var line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null) {
                    
                }
                        
            }
        }
    }
}
