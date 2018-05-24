using System;
using System.IO;
using System.Text;
using System.Xml;

namespace JudgeUtility
{
    partial class Program
    {
        static void AppendProblem(XmlNode root, StringBuilder sb, StringBuilder sb2, StringBuilder sb3)
        {
            var id = int.Parse(root.SelectSingleNode("id").InnerText);
            var title = root.SelectSingleNode("title").InnerText;
            sb3.AppendLine(id + " => '" + title + "',");
            sb.AppendLine("<div class=\"card\"><div class=\"card-body\">")
              .AppendLine("<h3 class=\"card-title\">" + title + "</h3>")
              .AppendLine("<p class=\"card-text\">" +
                "时间限制：" + root.SelectSingleNode("time_limit").InnerText + "ms，" +
                "内存限制：" + root.SelectSingleNode("memory_limit").InnerText + "MB。</p>")
              .AppendLine("<p class=\"card-text\">" + root.SelectSingleNode("description").InnerText + "</p>");
            sb2.Append("<div class=\"modal fade\" id=\"Modal" + id + "\" tabindex=\"-1\" role=\"dialog\">")
               .Append("<div class=\"modal-dialog\" role=\"document\"><div class=\"modal-content\">")
               .Append("<div class=\"modal-header\"><h5 class=\"modal-title\">" + title + "</h5>")
               .Append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span>&times;</span>")
               .AppendLine("</button></div><div class=\"modal-body\"><h4>样例</h4>");
            var samples = root.SelectSingleNode("samples").SelectNodes("group");
            for (int i = 0; i < samples.Count; i++)
            {
                var node = samples.Item(i);
                sb2.Append("<p class=\"mb-1 ml-2 mt-2\">输入</p><pre class=\"sample_io_area mb-0\">")
                   .Append(node.SelectSingleNode("input").InnerText)
                   .Append("</pre><p class=\"mb-1 ml-2 mt-2\">输出</p><pre class=\"sample_io_area\">")
                   .Append(node.SelectSingleNode("output").InnerText)
                   .AppendLine("</pre>");
            }
            sb2.Append("<h4>提示</h4><p>" + root.SelectSingleNode("hint").InnerText.Replace("\r\n", "<br>\r\n") + "</p>")
               .Append("</div><div class=\"modal-footer\"><button type=\"button\" class=\"btn btn-success\"")
               .AppendLine(" data-dismiss=\"modal\">关闭</button></div></div></div></div>");
            sb.Append("<a href=\"submit.php?id=" + id + "\" class=\"btn btn-primary mr-3\">尝试提交</a>")
              .Append("<button type=\"button\" class=\"btn btn-secondary\" data-toggle=\"modal\" ")
              .AppendLine("data-target=\"#Modal" + id + "\">样例与提示</button></div></div>");
        }

        static void ConfigCache()
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            var sb3 = new StringBuilder();
            sb3.Append("<?php $GLOBALS['problems'] = [");
            foreach (var file in Directory.EnumerateFiles(".\\prob\\", "*.xml"))
            {
                var doc = new XmlDocument();
                doc.Load(file);
                AppendProblem(doc.SelectSingleNode("problem"), sb, sb2, sb3);
            }
            sb3.Append("];");
            sb.Append(sb2.ToString());
            File.WriteAllText("..\\..\\JudgeWeb\\source\\prob_list.html", sb.ToString());
            File.WriteAllText("..\\..\\JudgeWeb\\source\\config_prob.php", sb3.ToString());
            sb.Clear();
            sb2.Clear();
        }
    }
}
