using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*

{"Profile":{"PublicKeyBase58Check":"BC1YLhF5DHfgqM7rwYK8PVnfKDmMXyVeQqJyeJ8YGsmbVb14qTm123G","Username":"weeblet","Description":"17, Science and Tech GEEK, Redditor and Discord Admin 〜(꒪꒳꒪)〜\nBuilding @Cordify and other things 💎\n\nhttps://wun.vc/id/weeblet","IsHidden":false,"IsReserved":false,"IsVerified":false,"Comments":null,"Posts":null,"CoinEntry":{"CreatorBasisPoints":690,"DeSoLockedNanos":10316470889,"NumberOfHolders":36,"CoinsInCirculationNanos":21764663365,"CoinWatermarkNanos":27291781127,"BitCloutLockedNanos":10316470889},"DAOCoinEntry":{"NumberOfHolders":5,"CoinsInCirculationNanos":"0x1039d90280","MintingDisabled":false,"TransferRestrictionStatus":"unrestricted"},"CoinPriceDeSoNanos":1422002961,"CoinPriceBitCloutNanos":1422002961,"UsersThatHODL":null,"IsFeaturedTutorialWellKnownCreator":false,"IsFeaturedTutorialUpAndComingCreator":false},"IsBlacklisted":false,"IsGraylisted":false}

*/


// {
//     "Profile" : { "Username": "weeblet"}
// }


[Serializable]
public class ProfileClass
{
    public string Username;
}

[Serializable]
public class SingleProfile
{
    public ProfileClass Profile;
}

