using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace M_Centres_2._0
{
    static class txt_Manager
    {
        public static FlowDocument Document { get; private set; }
        public static int ParagraphCount { get; private set; }
        public static List<Paragraph> Paragraphs { get; set; }
        public static int AddRun(Run v)
        {
            if (v.Text==(Environment.CurrentDirectory)||v.Text.Contains(Environment.CurrentDirectory+">"))
                return -1;
            Paragraphs.Last().Inlines.Add(v);
            return ParagraphCount;

        }
        public static int AddRun(int i,Run v)
        {
            Paragraphs[i].Inlines.Add(v);
            return ParagraphCount;

        }
        public static async Task AddStepHeading(string Heading,int i)
        {
            var s = new Run()
            {
                Text = "\n" + Heading ,
                FontSize = 25,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Colors.GhostWhite),
                Tag= i,
            };
            AddRun(s);
           
        }
        public static void AddStep(string Step, byte number,int i,bool show)
        {
            var s = new Run()
            {
                Text = "\n\t" + number.ToString() + ")  " + Step,
                Tag=i,
                
                Foreground = new SolidColorBrush(Colors.GhostWhite)
                
            };
            if (!show)
                s.Text = "\n\t   " + Step;
               
            AddRun(s);

        }
       public static async Task EditAt(int paragraph,Run data) {
            var h = Paragraphs[paragraph-1];
            int i = h.Inlines.Count;
            List<Run> cache = new List<Run>();
            for(int v = 0; v < i; v++)
            {
                cache.Add((Run)h.Inlines.FirstInline);
                h.Inlines.Remove(h.Inlines.FirstInline);

            }
            h.Inlines.Add(data);
            for (int v = 0; v < i; v++)
            {
                h.Inlines.Add(cache[v]);
              
            }
           

        }
        public static async Task AddErrorLine(string Step,int i)
        {
            var s = new Run()
            {
                Text = "\n\t"  + Step,
               Tag= -i,
                Foreground = new SolidColorBrush(Colors.IndianRed)

            };
            
            AddRun(s);
        }
        public static async Task CopyErrorToClipBoard(int id)
        {
            id = -id;
            List<Run> arg=new List<Run>();
           foreach(var i in Paragraphs)
            {
                arg = (from Run ida in i.Inlines where (int)ida.Tag == id select ida).ToList();
            }
            string output = "";
 ;           foreach(Run item in arg)
            {
                output += item.Text;

            }
            Clipboard.SetText(output);
        }
        public static async Task CopyLogToClipBoard(int id,int count) 
        {
          var  output = "";
            List<Run> arg = new List<Run>();

            List<Run> FullList = new List<Run>();
            Paragraph il = new Paragraph();
            foreach (var i in Paragraphs)
            {
                arg = (from Run ida in i.Inlines where (int)ida.Tag == id select ida).ToList();
           
                foreach(Run inl in i.Inlines)
                {
                    arg.Add(inl);
                }
            }
            int index = FullList.IndexOf(arg.First());
            for(int i = 0; i <= count; i++)
            {
                output += " " + FullList[index + i]; 
            }
            Clipboard.SetText(output);

            
           



        }

       public static async Task AddErrorHeading(string Heading,int i)
        {
            var s = new Run()
            {
                Text = "\n" + Heading,
                FontSize = 25,
                Tag=-i,
                FontWeight = FontWeights.SemiBold,
           
                Foreground = new SolidColorBrush(Colors.IndianRed)

            };
            AddRun(s);
        }
        public static void RemoveAt(int i)
        {
            var ia = Paragraphs[i];
            Document.Blocks.Remove(ia);
            Paragraphs.Remove(ia);
        }
       
        public static int AddParagraph(Run[] data)
        {
            Paragraph io = new Paragraph();
            int v = data.Length;
            for(int i = 0; i < v; ++i)
            {
                io.Inlines.Add(data[i]);
            }
            ParagraphCount++;
         Paragraphs.Add(io);
            Document.Blocks.Add(io);
            return ParagraphCount;
        }
     public static void ClearAll()
        {
            ParagraphCount = -1;
           foreach(var i in Paragraphs)
            {
                Document.Blocks.Remove(i);
                Paragraphs.Remove(i);
            }
            AddParagraph(new Run[1]);
        }
        
        public static void initialize(ref RichTextBox arg)
        {
            Paragraphs = new List<Paragraph>();
            Document = new FlowDocument();
            AddParagraph(new Run[] { new Run("")});

            arg.Document = Document;
            ParagraphCount = -1;
            
        }
      

        
    }
}
