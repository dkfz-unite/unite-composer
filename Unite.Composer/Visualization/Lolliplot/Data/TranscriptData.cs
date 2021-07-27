using System;
using System.Collections.Generic;
using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Visualization.Lolliplot.Data
{
    public class TranscriptData
    {
        /// <summary>
        /// Represents the display value of the transcript. <see cref="TranscriptIndex.Symbol"/>
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Represents the identifier of the transcript. <see cref="TranscriptIndex.Id"/>
        /// </summary>
        public long Value { get; set; }


        private sealed class LabelRelationalComparer : IComparer<TranscriptData>
        {
            public int Compare(TranscriptData x, TranscriptData y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Label, y.Label, StringComparison.Ordinal);
            }
        }

        public static IComparer<TranscriptData> LabelComparer { get; } = new LabelRelationalComparer();
    }
}