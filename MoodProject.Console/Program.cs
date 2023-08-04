// See https://aka.ms/new-console-template for more information

using MoodProject.Console;

Console.WriteLine("Hello, World!");

var q1 = new Question() {prop1 = "prop1", prop2 = "prop2"};

AnswerableQuestion q2 = new AnswerableQuestion();

Console.WriteLine(q2.prop1);
