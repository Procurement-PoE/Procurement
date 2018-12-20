using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class EssenceStashViewModel : ObservableBase
    {
        private readonly Dictionary<Item, ItemDisplayViewModel> _stash;

        public EssenceStashViewModel(Dictionary<Item, ItemDisplayViewModel> stashByLocation)
        {
            _stash = stashByLocation;
        }

        public ItemDisplayViewModel WhisperingGreed => GetEssenceItem(EssenceType.WhisperingGreed);
        public ItemDisplayViewModel WhisperingContempt => GetEssenceItem(EssenceType.WhisperingContempt);
        public ItemDisplayViewModel WhisperingHatred => GetEssenceItem(EssenceType.WhisperingHatred);
        public ItemDisplayViewModel WhisperingWoe => GetEssenceItem(EssenceType.WhisperingWoe);

        public ItemDisplayViewModel MutteringGreed => GetEssenceItem(EssenceType.MutteringGreed);
        public ItemDisplayViewModel MutteringContempt => GetEssenceItem(EssenceType.MutteringContempt);
        public ItemDisplayViewModel MutteringHatred => GetEssenceItem(EssenceType.MutteringHatred);
        public ItemDisplayViewModel MutteringWoe => GetEssenceItem(EssenceType.MutteringWoe);
        public ItemDisplayViewModel MutteringFear => GetEssenceItem(EssenceType.MutteringFear);
        public ItemDisplayViewModel MutteringAnger => GetEssenceItem(EssenceType.MutteringAnger);
        public ItemDisplayViewModel MutteringTorment => GetEssenceItem(EssenceType.MutteringTorment);
        public ItemDisplayViewModel MutteringSorrow => GetEssenceItem(EssenceType.MutteringSorrow);

        public ItemDisplayViewModel WeepingGreed => GetEssenceItem(EssenceType.WeepingGreed);
        public ItemDisplayViewModel WeepingContempt => GetEssenceItem(EssenceType.WeepingContempt);
        public ItemDisplayViewModel WeepingHatred => GetEssenceItem(EssenceType.WeepingHatred);
        public ItemDisplayViewModel WeepingWoe => GetEssenceItem(EssenceType.WeepingWoe);
        public ItemDisplayViewModel WeepingFear => GetEssenceItem(EssenceType.WeepingFear);
        public ItemDisplayViewModel WeepingAnger => GetEssenceItem(EssenceType.WeepingAnger);
        public ItemDisplayViewModel WeepingTorment => GetEssenceItem(EssenceType.WeepingTorment);
        public ItemDisplayViewModel WeepingSorrow => GetEssenceItem(EssenceType.WeepingSorrow);
        public ItemDisplayViewModel WeepingRage => GetEssenceItem(EssenceType.WeepingRage);
        public ItemDisplayViewModel WeepingSuffering => GetEssenceItem(EssenceType.WeepingSuffering);
        public ItemDisplayViewModel WeepingWrath => GetEssenceItem(EssenceType.WeepingWrath);
        public ItemDisplayViewModel WeepingDoubt => GetEssenceItem(EssenceType.WeepingDoubt);

        public ItemDisplayViewModel WailingGreed => GetEssenceItem(EssenceType.WailingGreed);
        public ItemDisplayViewModel WailingContempt => GetEssenceItem(EssenceType.WailingContempt);
        public ItemDisplayViewModel WailingHatred => GetEssenceItem(EssenceType.WailingHatred);
        public ItemDisplayViewModel WailingWoe => GetEssenceItem(EssenceType.WailingWoe);
        public ItemDisplayViewModel WailingFear => GetEssenceItem(EssenceType.WailingFear);
        public ItemDisplayViewModel WailingAnger => GetEssenceItem(EssenceType.WailingAnger);
        public ItemDisplayViewModel WailingTorment => GetEssenceItem(EssenceType.WailingTorment);
        public ItemDisplayViewModel WailingSorrow => GetEssenceItem(EssenceType.WailingSorrow);
        public ItemDisplayViewModel WailingRage => GetEssenceItem(EssenceType.WailingRage);
        public ItemDisplayViewModel WailingSuffering => GetEssenceItem(EssenceType.WailingSuffering);
        public ItemDisplayViewModel WailingWrath => GetEssenceItem(EssenceType.WailingWrath);
        public ItemDisplayViewModel WailingDoubt => GetEssenceItem(EssenceType.WailingDoubt);
        public ItemDisplayViewModel WailingLoathing => GetEssenceItem(EssenceType.WailingLoathing);
        public ItemDisplayViewModel WailingZeal => GetEssenceItem(EssenceType.WailingZeal);
        public ItemDisplayViewModel WailingAnguish => GetEssenceItem(EssenceType.WailingAnguish);
        public ItemDisplayViewModel WailingSpite => GetEssenceItem(EssenceType.WailingSpite);

        public ItemDisplayViewModel ScreamingGreed => GetEssenceItem(EssenceType.ScreamingGreed);
        public ItemDisplayViewModel ScreamingContempt => GetEssenceItem(EssenceType.ScreamingContempt);
        public ItemDisplayViewModel ScreamingHatred => GetEssenceItem(EssenceType.ScreamingHatred);
        public ItemDisplayViewModel ScreamingWoe => GetEssenceItem(EssenceType.ScreamingWoe);
        public ItemDisplayViewModel ScreamingFear => GetEssenceItem(EssenceType.ScreamingFear);
        public ItemDisplayViewModel ScreamingAnger => GetEssenceItem(EssenceType.ScreamingAnger);
        public ItemDisplayViewModel ScreamingTorment => GetEssenceItem(EssenceType.ScreamingTorment);
        public ItemDisplayViewModel ScreamingSorrow => GetEssenceItem(EssenceType.ScreamingSorrow);
        public ItemDisplayViewModel ScreamingRage => GetEssenceItem(EssenceType.ScreamingRage);
        public ItemDisplayViewModel ScreamingSuffering => GetEssenceItem(EssenceType.ScreamingSuffering);
        public ItemDisplayViewModel ScreamingWrath => GetEssenceItem(EssenceType.ScreamingWrath);
        public ItemDisplayViewModel ScreamingDoubt => GetEssenceItem(EssenceType.ScreamingDoubt);
        public ItemDisplayViewModel ScreamingLoathing => GetEssenceItem(EssenceType.ScreamingLoathing);
        public ItemDisplayViewModel ScreamingZeal => GetEssenceItem(EssenceType.ScreamingZeal);
        public ItemDisplayViewModel ScreamingAnguish => GetEssenceItem(EssenceType.ScreamingAnguish);
        public ItemDisplayViewModel ScreamingSpite => GetEssenceItem(EssenceType.ScreamingSpite);
        public ItemDisplayViewModel ScreamingScorn => GetEssenceItem(EssenceType.ScreamingScorn);
        public ItemDisplayViewModel ScreamingEnvy => GetEssenceItem(EssenceType.ScreamingEnvy);
        public ItemDisplayViewModel ScreamingMisery => GetEssenceItem(EssenceType.ScreamingMisery);
        public ItemDisplayViewModel ScreamingDread => GetEssenceItem(EssenceType.ScreamingDread);

        public ItemDisplayViewModel ShriekingGreed => GetEssenceItem(EssenceType.ShriekingGreed);
        public ItemDisplayViewModel ShriekingContempt => GetEssenceItem(EssenceType.ShriekingContempt);
        public ItemDisplayViewModel ShriekingHatred => GetEssenceItem(EssenceType.ShriekingHatred);
        public ItemDisplayViewModel ShriekingWoe => GetEssenceItem(EssenceType.ShriekingWoe);
        public ItemDisplayViewModel ShriekingFear => GetEssenceItem(EssenceType.ShriekingFear);
        public ItemDisplayViewModel ShriekingAnger => GetEssenceItem(EssenceType.ShriekingAnger);
        public ItemDisplayViewModel ShriekingTorment => GetEssenceItem(EssenceType.ShriekingTorment);
        public ItemDisplayViewModel ShriekingSorrow => GetEssenceItem(EssenceType.ShriekingSorrow);
        public ItemDisplayViewModel ShriekingRage => GetEssenceItem(EssenceType.ShriekingRage);
        public ItemDisplayViewModel ShriekingSuffering => GetEssenceItem(EssenceType.ShriekingSuffering);
        public ItemDisplayViewModel ShriekingWrath => GetEssenceItem(EssenceType.ShriekingWrath);
        public ItemDisplayViewModel ShriekingDoubt => GetEssenceItem(EssenceType.ShriekingDoubt);
        public ItemDisplayViewModel ShriekingLoathing => GetEssenceItem(EssenceType.ShriekingLoathing);
        public ItemDisplayViewModel ShriekingZeal => GetEssenceItem(EssenceType.ShriekingZeal);
        public ItemDisplayViewModel ShriekingAnguish => GetEssenceItem(EssenceType.ShriekingAnguish);
        public ItemDisplayViewModel ShriekingSpite => GetEssenceItem(EssenceType.ShriekingSpite);
        public ItemDisplayViewModel ShriekingScorn => GetEssenceItem(EssenceType.ShriekingScorn);
        public ItemDisplayViewModel ShriekingEnvy => GetEssenceItem(EssenceType.ShriekingEnvy);
        public ItemDisplayViewModel ShriekingMisery => GetEssenceItem(EssenceType.ShriekingMisery);
        public ItemDisplayViewModel ShriekingDread => GetEssenceItem(EssenceType.ShriekingDread);

        public ItemDisplayViewModel DeafeningGreed => GetEssenceItem(EssenceType.DeafeningGreed);
        public ItemDisplayViewModel DeafeningContempt => GetEssenceItem(EssenceType.DeafeningContempt);
        public ItemDisplayViewModel DeafeningHatred => GetEssenceItem(EssenceType.DeafeningHatred);
        public ItemDisplayViewModel DeafeningWoe => GetEssenceItem(EssenceType.DeafeningWoe);
        public ItemDisplayViewModel DeafeningFear => GetEssenceItem(EssenceType.DeafeningFear);
        public ItemDisplayViewModel DeafeningAnger => GetEssenceItem(EssenceType.DeafeningAnger);
        public ItemDisplayViewModel DeafeningTorment => GetEssenceItem(EssenceType.DeafeningTorment);
        public ItemDisplayViewModel DeafeningSorrow => GetEssenceItem(EssenceType.DeafeningSorrow);
        public ItemDisplayViewModel DeafeningRage => GetEssenceItem(EssenceType.DeafeningRage);
        public ItemDisplayViewModel DeafeningSuffering => GetEssenceItem(EssenceType.DeafeningSuffering);
        public ItemDisplayViewModel DeafeningWrath => GetEssenceItem(EssenceType.DeafeningWrath);
        public ItemDisplayViewModel DeafeningDoubt => GetEssenceItem(EssenceType.DeafeningDoubt);
        public ItemDisplayViewModel DeafeningLoathing => GetEssenceItem(EssenceType.DeafeningLoathing);
        public ItemDisplayViewModel DeafeningZeal => GetEssenceItem(EssenceType.DeafeningZeal);
        public ItemDisplayViewModel DeafeningAnguish => GetEssenceItem(EssenceType.DeafeningAnguish);
        public ItemDisplayViewModel DeafeningSpite => GetEssenceItem(EssenceType.DeafeningSpite);
        public ItemDisplayViewModel DeafeningScorn => GetEssenceItem(EssenceType.DeafeningScorn);
        public ItemDisplayViewModel DeafeningEnvy => GetEssenceItem(EssenceType.DeafeningEnvy);
        public ItemDisplayViewModel DeafeningMisery => GetEssenceItem(EssenceType.DeafeningMisery);
        public ItemDisplayViewModel DeafeningDread => GetEssenceItem(EssenceType.DeafeningDread);

        public ItemDisplayViewModel Insanity => GetEssenceItem(EssenceType.Insanity);
        public ItemDisplayViewModel Horror => GetEssenceItem(EssenceType.Horror);
        public ItemDisplayViewModel Delirium => GetEssenceItem(EssenceType.Delirium);
        public ItemDisplayViewModel Hysteria => GetEssenceItem(EssenceType.Hysteria);

        public ItemDisplayViewModel RemnantOfCorruption => GetEssenceItem(EssenceType.RemnantOfCorruption);

        private ItemDisplayViewModel GetItemAtPosition(int x, int y)
        {
            var item = _stash.FirstOrDefault(i => i.Key.X == x && i.Key.Y == y).Key;
            
            //We don't have an essence you are looking for
            if(item == null)
                return new ItemDisplayViewModel(null);

            if (_stash.ContainsKey(item) == false)
                _stash.Add(item, new ItemDisplayViewModel(item));

            return _stash[item];
        }

        private ItemDisplayViewModel GetEssenceItem(EssenceType essenceType)
        {
            ItemDisplayViewModel rtnViewModel = null;

            foreach (var item in _stash)
            {
                var essence = item.Key as Essence;

                if (essence?.Type == essenceType)
                {
                    rtnViewModel = new ItemDisplayViewModel(essence);

                    _stash[essence] = rtnViewModel;
                    break;
                }
            }

            return rtnViewModel ?? (rtnViewModel = new ItemDisplayViewModel(null));
        }
    }
}