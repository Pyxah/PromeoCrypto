using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace CryptoClient
{
    public partial class MainWindow : Window
    {
        private const int ServerPort = 6666;
        private TcpClient client;
        private const string ServerIp = "172.18.1.68";
    

        public MainWindow()
        {
            InitializeComponent();
           

        }
        private void ClearTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EncryptButton_Click(sender, e);
            }
        }
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                 
                string plaintext = ClearTextBox.Text.ToUpper().Replace(" ", "");

                if (!Regex.IsMatch(plaintext, @"^[A-Z]+$"))
                {
                    MessageBox.Show("La chaîne saisie ne doit contenir que des lettres et des espaces.");
                    return;
                }

                string method;
                if (CaesarRadioButton.IsChecked == true)
                {
                    method = "C";
                }
                else if (PlayfairRadioButton.IsChecked == true)
                {
                    method = "P";
                }
                else if (SubstitutionRadioButton.IsChecked == true)
                {
                    method = "S";
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner une méthode de chiffrement.");
                    return;
                }
                try
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort));
                byte[] requestData = Encoding.ASCII.GetBytes(method + " " + plaintext);
                client.Send(requestData);

                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = client.Receive(buffer);

                string responseData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                EncryptedTextBox.Text = responseData;
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
    catch (SocketException ex)
            {
                // Afficher une boîte de message avec l'erreur
                MessageBox.Show($"Une erreur réseau s'est produite : {ex.Message}");
            }
            catch (Exception ex)
            {
                // Afficher une boîte de message avec l'erreur
                MessageBox.Show($"Une erreur inattendue s'est produite : {ex.Message}");
            }
        }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Pas besoin de fermer le client ici car il est déjà fermé grâce au 'using' dans la méthode EncryptButton_Click
        }
    }
}

