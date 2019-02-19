﻿namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class CopyHostnameKeyTappedHandler
    {
        public CopyHostnameKeyTappedHandler(
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<UiReaderWriter, ClipboardCopier>(
                (uiRw, copier) =>
                {
                    uiRw.Write(
                        ui,
                        () => copier.Copy(
                            ui.Hostname));
                });
        }

        protected readonly MethodWeb web;
    }
}