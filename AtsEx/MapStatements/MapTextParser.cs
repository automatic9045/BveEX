using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.MapStatements
{
    internal static class MapTextParser
    {
        public static IEnumerable<TextWithPosition> GetStatementsFromText(string text)
        {
            string trimmedText = text.ToLowerInvariant();

            {
                bool isInString = false;

                int lineIndex = 1;
                int notTrimmedLastLineBreakIndex = -1;

                int lastStatementEndIndex = -1;
                int notTrimmedLastStatementEndIndex = trimmedText.Length - trimmedText.TrimStart().Length - 1;

                int i = 0;
                int n = 0;
                while (i < trimmedText.Length)
                {
                    switch (trimmedText[i])
                    {
                        case '\n':
                            isInString = false;

                            lineIndex++;
                            notTrimmedLastLineBreakIndex = n;

                            lastStatementEndIndex = i;
                            notTrimmedLastStatementEndIndex = n;
                            break;

                        case '/':
                            if (!isInString && i + 1 < text.Length && text[i + 1] == '/')
                            {
                                int nextLineBreakIndex = text.IndexOf('\n', i);
                                if (nextLineBreakIndex == -1)
                                {
                                    yield break;
                                }
                                else
                                {
                                    i = nextLineBreakIndex;
                                }
                            }
                            break;

                        case '#':
                            yield break;

                        case '\'':
                            isInString = !isInString;
                            break;

                        case ' ':
                        case '\t':
                            if (!isInString)
                            {
                                trimmedText = trimmedText.Remove(i, 1);
                                i--;
                            }
                            break;

                        case ';':
                            if (!isInString)
                            {
                                trimmedText = trimmedText.Remove(i, 1);
                                i--;
                                if (i != lastStatementEndIndex)
                                {
                                    string statementText = trimmedText.Substring(lastStatementEndIndex + 1, i - lastStatementEndIndex);
                                    string notTrimmedStatementText = text.Substring(notTrimmedLastStatementEndIndex + 1, n - notTrimmedLastStatementEndIndex);

                                    int headSpaceCount = notTrimmedStatementText.Length - notTrimmedStatementText.TrimStart().Length;
                                    yield return new TextWithPosition(lineIndex, notTrimmedLastStatementEndIndex + headSpaceCount + 1 - notTrimmedLastLineBreakIndex, statementText);
                                }

                                lastStatementEndIndex = i;
                                notTrimmedLastStatementEndIndex = n;
                            }
                            break;
                    }

                    i++;
                    n++;
                }
            }
        }

        internal class TextWithPosition
        {
            public int LineIndex { get; }
            public int CharIndex { get; }
            public string Text { get; }

            public TextWithPosition(int lineIndex, int charIndex, string text)
            {
                LineIndex = lineIndex;
                CharIndex = charIndex;
                Text = text;
            }

            public override string ToString() => $"({LineIndex}, {CharIndex}): \"{Text}\"";
        }
    }
}
