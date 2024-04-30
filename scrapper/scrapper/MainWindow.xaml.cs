using System;
using System.IO;
using System.Net;
using System.Windows;
using Microsoft.Win32;


namespace scrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = urll.Text;
            string fileName = fileNamee.Text;


            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(fileName))
            {
                messageTextBlock.Text = "Please enter both URL and File Name.";
                return;
            }

            try
            {
                // Download the content from URL
                WebClient client = new WebClient();
                byte[] content = client.DownloadData(url);

                // Extract file extension from filename
                string extension = Path.GetExtension(fileName);

                // Let user choose the file path using SaveFileDialog
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = $"All Files (*.*)|*.*|{extension} files (*{extension})|*{extension}";
                saveDialog.DefaultExt = extension;

                if (saveDialog.ShowDialog() == true)
                {
                    string filePath = saveDialog.FileName;
                    string directoryPath = Path.GetDirectoryName(filePath);

                    // Check if directory exists, create it if needed
                    if (!Directory.Exists(directoryPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        catch (Exception ex)
                        {
                            messageTextBlock.Text = $"Error creating directory: {ex.Message}";
                            return;
                        }
                    }

                    try
                    {
                        // Write downloaded content to the chosen file
                        File.WriteAllBytes(filePath, content);
                        messageTextBlock.Text = $"File '{Path.GetFileName(filePath)}' generated successfully.";
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        messageTextBlock.Text = $"Error saving file: Insufficient permissions for '{filePath}'.";
                    }
                    catch (IOException ex)
                    {
                        messageTextBlock.Text = $"Error saving file: {ex.Message}";
                    }
                }
            }
            catch (WebException ex)
            {
                messageTextBlock.Text = $"Error downloading file: {ex.Message}";
            }
        }

    }
}
