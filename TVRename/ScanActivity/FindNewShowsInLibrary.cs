// 
// Main website for TVRename is http://tvrename.com
// 
// Source code available at https://github.com/TV-Rename/tvrename
// 
// This code is released under GPLv3 https://github.com/TV-Rename/tvrename/blob/master/LICENSE.md
// 

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TVRename
{
    internal class FindNewShowsInLibrary : ScanActivity
    {
        public FindNewShowsInLibrary(TVDoc doc) : base(doc)
        {
        }

        protected override void Check(SetProgressDelegate prog, ICollection<ShowItem> showList, TVDoc.ScanSettings settings)
        {
            BulkAddManager bam = new BulkAddManager(MDoc);
            bam.CheckFolders(settings.Token, prog,false);
            foreach (FoundFolder folder in bam.AddItems)
            {
                if (settings.Token.IsCancellationRequested)
                    break;

                if (folder.CodeKnown)
                    continue;

                BulkAddManager.GuessShowItem(folder, MDoc.Library);

                if (folder.CodeKnown)
                    continue;

                FolderMonitorEdit ed = new FolderMonitorEdit(folder);
                if ((ed.ShowDialog() != DialogResult.OK) || (ed.Code == -1))
                    continue;

                folder.TVDBCode = ed.Code;
            }

            if (!bam.AddItems.Any(s => s.CodeKnown)) return;

            bam.AddAllToMyShows();
            LOGGER.Info("Added new shows called: {0}", string.Join(",", bam.AddItems.Where(s => s.CodeKnown).Select(s => s.Folder)));

            MDoc.SetDirty();
            MDoc.DoDownloadsFG();
            MDoc.DoWhenToWatch(true);

            MDoc.WriteUpcoming();
            MDoc.WriteRecent();
        }

        public override bool Active() => TVSettings.Instance.DoBulkAddInScan;
    }
}