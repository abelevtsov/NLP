using System;
using System.IO;

using java.util;
using java.io;
using edu.stanford.nlp.pipeline;

using static System.Console;

namespace NLP
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.6.0-models.jar`
            var modelPath = args[0];

            // Text for processing
            var text = args[1];

            var props = new Properties();

            props.setProperty("annotators", $"{NlpAction.tokenize | NlpAction.ssplit | NlpAction.pos | NlpAction.lemma | NlpAction.ner | NlpAction.parse | NlpAction.dcoref}");
            props.setProperty("ner.useSUTime", 0.ToString());

            var pipeline = InitPipeline(modelPath, props);

            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                WriteLine(stream.toString());
                stream.close();
            }

            ReadKey();
        }

        /// <summary>
        /// We should temporary change current directory, so StanfordCoreNLP could find all the model files automatically
        /// </summary>
        private static StanfordCoreNLP InitPipeline(string modelPath, Properties props)
        {
            var currentDir = Environment.CurrentDirectory;

            Directory.SetCurrentDirectory(modelPath);
            var pipeline = new StanfordCoreNLP(props);

            Directory.SetCurrentDirectory(currentDir);

            return pipeline;
        }

        [Flags]
        private enum NlpAction
        {
            tokenize = 0x1,
            ssplit = 0x2,
            pos = 0x4,
            lemma = 0x8,
            ner = 0x10,
            parse = 0x20,
            dcoref = 0x40
        }
    }
}
