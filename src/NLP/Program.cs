﻿using System;
using System.Collections.Generic;
using System.IO;

using java.util;
using java.io;
using edu.stanford.nlp.pipeline;

using Console = System.Console;

namespace NLP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.6.0-models.jar`
            var jarRoot = @"C:\Users\abelevtsov\Documents\GitHub\NLP\stanford-corenlp-full-2015-12-09\stanford-corenlp-3.6.0-models";

            // Text for processing
            var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref"); // ToDo: NlpAction.tokenize | NlpAction.ssplit | NlpAction.pos | e.t.c.
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }

            Console.ReadKey();
        }

        [Flags]
        private enum NlpAction
        {
            tokenize = 1,
            ssplit = 2,
            pos = 4,
            lemma = 8,
            ner = 16,
            parse = 32,
            dcoref = 64,
            all = tokenize | ssplit | pos | lemma | ner | parse | dcoref
        }
    }
}
