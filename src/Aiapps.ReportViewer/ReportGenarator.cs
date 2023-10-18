using Aiapps.Reporting;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;

namespace Aiapps.ReportViewer
{
    public class ReportGenarator : IReportGenerator
    {
        public byte[] Generate(string reportPath, string logoPath, IDictionary<string, object[]> dataSources, string format, out string extension)
        {
            return Generate(reportPath,
                logoPath,
                new Dictionary<string, string>(),
                dataSources,
                format,
                out extension);
        }

        public byte[] Generate(string reportPath, byte[] logo, IDictionary<string, object[]> dataSources, string format, out string extension)
        {
            var logoBase64 = "No Logo";
            if (logo != null)
                logoBase64 = Convert.ToBase64String(logo);
            return Generate(reportPath,
                logoBase64,
                new Dictionary<string, string>(),
                dataSources,
                format,
                out extension);
        }
        public byte[] Generate(string reportPath, byte[] logo, IDictionary<string, string> paramenters, IDictionary<string, object[]> dataSources, string format, out string extension)
        {

            var logoBase64 = "No Logo";
            if (logo != null)
                logoBase64 = Convert.ToBase64String(logo);
            return Generate(reportPath,
                logoBase64,
                paramenters,
                dataSources,
                format,
                out extension);
        }

        public byte[] Generate(string reportPath, string logo, IDictionary<string, string> paramenters, IDictionary<string, object[]> dataSources, string format, out string extension)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;

                var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableHyperlinks = true;
                viewer.LocalReport.ReportPath = reportPath;
                viewer.LocalReport.EnableExternalImages = true;

                foreach (var d in paramenters)
                {
                    viewer.LocalReport.SetParameters(new ReportParameter(d.Key, d.Value));
                }

                ConfigureLogoParameter(logo, viewer);

                foreach (var d in dataSources)
                {
                    var reportDataSource = new ReportDataSource(d.Key, d.Value);
                    viewer.LocalReport.DataSources.Add(reportDataSource);
                }
                byte[] bytes = viewer.LocalReport.Render(
                        format, null, out mimeType, out encoding, out extension,
                        out streamids, out warnings);
                viewer.Dispose();
                viewer = null;
                return bytes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConfigureLogoParameter(string logo, Microsoft.Reporting.WebForms.ReportViewer viewer)
        {
            if (!string.IsNullOrWhiteSpace(logo))
            {
                if (logo.Contains("http"))
                {
                    viewer.LocalReport.SetParameters(new ReportParameter("ImagePath", logo));
                }
                else
                {
                    viewer.LocalReport.SetParameters(new ReportParameter("ReportLogo", logo));
                }
            }
        }
    }
}
