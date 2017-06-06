using System;
using Solum.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(NoScrollBarEffect), "NoScrollBarEffect")]
namespace Solum.iOS.Effects
{
    public class NoScrollBarEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control is UICollectionView collectionView)
            {
                collectionView.ShowsVerticalScrollIndicator = false;
                collectionView.ShowsHorizontalScrollIndicator = false;
            }
        }

        protected override void OnDetached()
        {
            if (Control is UICollectionView collectionView)
            {
                collectionView.ShowsVerticalScrollIndicator = true;
                collectionView.ShowsHorizontalScrollIndicator = true;
            }
        }
    }
}
