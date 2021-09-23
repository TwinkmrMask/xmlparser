﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Xml;
using System;

namespace XmlParser
{
    public partial class MainWindow : IDefaultSettings
    {
        private readonly List<string> _fileNames = new();
        private string _directoryName;
        private string _fileName;

        private static void Copy(string text) => Clipboard.SetText(text);
        private static void GetPath(out string directoryName, out string fileName)
        {
            try
            {
                var openDialog = new Microsoft.Win32.OpenFileDialog { Filter = "Файл Xml|*.xml" };
                openDialog.ShowDialog();
                directoryName = Path.GetDirectoryName(openDialog.FileName);
                fileName = openDialog.FileName;
            }
            catch (Exception exception1)
            {
                if (MessageBox.Show( $"Ошибка - {exception1.HResult}\nНажмите OK чтобы скопировать код ошибки, или нажмите Отмена чтобы переписать самостоятельно",
                    "Ошибка получения пути", MessageBoxButton.OKCancel,
                    MessageBoxImage.Error) == MessageBoxResult.OK)
                    Copy(exception1.HResult.ToString());
                directoryName = default;
                fileName = default;
            }
        }
        private void GetFile()
        {
            try
            {
                var allFiles = Directory.EnumerateFiles(_directoryName, "*.xml");
                foreach (var filename in allFiles) _fileNames.Add(filename);
            }

            catch (Exception exception4)
            {
                if (MessageBox.Show($"Ошибка - {exception4.HResult}\nНажмите OK чтобы скопировать код ошибки, или нажмите Отмена чтобы переписать самостоятельно",
                    "Неизвестная ошибка", MessageBoxButton.OKCancel,
                    MessageBoxImage.Error) == MessageBoxResult.OK)
                    Copy(exception4.HResult.ToString());

            }
        }
        private void SetSource()
        {
            try
            {
                GetPath(out this._directoryName, out this._fileName);
                if ( new[] { _directoryName, _fileName }.Any(string.IsNullOrWhiteSpace))
                    throw new IOException("Null path");
                GetFile();
                var declarations = _fileNames.Select(item => new Content 
                    { FileName = item }).ToList();
                Data.ItemsSource = declarations;
            }

            catch (IOException e) when (e.Message == "Null path")
            {
                if (MessageBox.Show("Выберите файл или закройте приложение", "Пустой путь", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
                    SetSource();
                else
                    this.Close();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            SetSource();
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Data.SelectedItem is not Content path) return;
            var info = new Information(path.FileName);
            AddFile(path.FileName);
            info.Show();
        }
        private static void AddFile(string name)
        {
            using var reader = XmlReader.Create(name);
            if (reader.IsEmptyElement) return;
            using var adapter = new XmlAdapter(Path.GetFileName(name));
            adapter.CreateLink(reader.ReadInnerXml());
            foreach (var variable in adapter.GetAllFileNames())
                MessageBox.Show(variable);
        }
        
    }
}
