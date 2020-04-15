using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionProcessor
{
    public enum EventCodes : short
    {
        // Token: 0x040038FD RID: 14589
        Leave = 1,
        // Token: 0x040038FE RID: 14590
        JoinFinished,
        // Token: 0x040038FF RID: 14591
        Move,
        // Token: 0x04003900 RID: 14592
        Teleport,
        // Token: 0x04003901 RID: 14593
        ChangeEquipment,
        // Token: 0x04003902 RID: 14594
        HealthUpdate,
        // Token: 0x04003903 RID: 14595
        EnergyUpdate,
        // Token: 0x04003904 RID: 14596
        DamageShieldUpdate,
        // Token: 0x04003905 RID: 14597
        CraftingFocusUpdate,
        // Token: 0x04003906 RID: 14598
        ActiveSpellEffectsUpdate,
        // Token: 0x04003907 RID: 14599
        ResetCooldowns,
        // Token: 0x04003908 RID: 14600
        Attack,
        // Token: 0x04003909 RID: 14601
        CastStart,
        // Token: 0x0400390A RID: 14602
        CastCancel,
        // Token: 0x0400390B RID: 14603
        CastTimeUpdate,
        // Token: 0x0400390C RID: 14604
        CastFinished,
        // Token: 0x0400390D RID: 14605
        CastSpell,
        // Token: 0x0400390E RID: 14606
        CastHit,
        // Token: 0x0400390F RID: 14607
        CastHits,
        // Token: 0x04003910 RID: 14608
        ChannelingEnded,
        // Token: 0x04003911 RID: 14609
        AttackBuilding,
        // Token: 0x04003912 RID: 14610
        InventoryPutItem,
        // Token: 0x04003913 RID: 14611
        InventoryDeleteItem,
        // Token: 0x04003914 RID: 14612
        NewCharacter,
        // Token: 0x04003915 RID: 14613
        NewEquipmentItem,
        // Token: 0x04003916 RID: 14614
        NewSimpleItem,
        // Token: 0x04003917 RID: 14615
        NewFurnitureItem,
        // Token: 0x04003918 RID: 14616
        NewJournalItem,
        // Token: 0x04003919 RID: 14617
        NewLaborerItem,
        // Token: 0x0400391A RID: 14618
        NewSimpleHarvestableObject,
        // Token: 0x0400391B RID: 14619
        NewSimpleHarvestableObjectList,
        // Token: 0x0400391C RID: 14620
        NewHarvestableObject,
        // Token: 0x0400391D RID: 14621
        NewSilverObject,
        // Token: 0x0400391E RID: 14622
        NewBuilding,
        // Token: 0x0400391F RID: 14623
        HarvestableChangeState,
        // Token: 0x04003920 RID: 14624
        MobChangeState,
        // Token: 0x04003921 RID: 14625
        FactionBuildingInfo,
        // Token: 0x04003922 RID: 14626
        CraftBuildingInfo,
        // Token: 0x04003923 RID: 14627
        RepairBuildingInfo,
        // Token: 0x04003924 RID: 14628
        MeldBuildingInfo,
        // Token: 0x04003925 RID: 14629
        ConstructionSiteInfo,
        // Token: 0x04003926 RID: 14630
        PlayerBuildingInfo,
        // Token: 0x04003927 RID: 14631
        FarmBuildingInfo,
        // Token: 0x04003928 RID: 14632
        TutorialBuildingInfo,
        // Token: 0x04003929 RID: 14633
        LaborerObjectInfo,
        // Token: 0x0400392A RID: 14634
        LaborerObjectJobInfo,
        // Token: 0x0400392B RID: 14635
        MarketPlaceBuildingInfo,
        // Token: 0x0400392C RID: 14636
        HarvestStart,
        // Token: 0x0400392D RID: 14637
        HarvestCancel,
        // Token: 0x0400392E RID: 14638
        HarvestFinished,
        // Token: 0x0400392F RID: 14639
        TakeSilver,
        // Token: 0x04003930 RID: 14640
        ActionOnBuildingStart,
        // Token: 0x04003931 RID: 14641
        ActionOnBuildingCancel,
        // Token: 0x04003932 RID: 14642
        ActionOnBuildingFinished,
        // Token: 0x04003933 RID: 14643
        ItemRerollQualityStart,
        // Token: 0x04003934 RID: 14644
        ItemRerollQualityCancel,
        // Token: 0x04003935 RID: 14645
        ItemRerollQualityFinished,
        // Token: 0x04003936 RID: 14646
        InstallResourceStart,
        // Token: 0x04003937 RID: 14647
        InstallResourceCancel,
        // Token: 0x04003938 RID: 14648
        InstallResourceFinished,
        // Token: 0x04003939 RID: 14649
        CraftItemFinished,
        // Token: 0x0400393A RID: 14650
        LogoutCancel,
        // Token: 0x0400393B RID: 14651
        ChatMessage,
        // Token: 0x0400393C RID: 14652
        ChatSay,
        // Token: 0x0400393D RID: 14653
        ChatWhisper,
        // Token: 0x0400393E RID: 14654
        ChatMuted,
        // Token: 0x0400393F RID: 14655
        PlayEmote,
        // Token: 0x04003940 RID: 14656
        StopEmote,
        // Token: 0x04003941 RID: 14657
        SystemMessage,
        // Token: 0x04003942 RID: 14658
        UtilityTextMessage,
        // Token: 0x04003943 RID: 14659
        UpdateMoney,
        // Token: 0x04003944 RID: 14660
        UpdateFame,
        // Token: 0x04003945 RID: 14661
        UpdateLearningPoints,
        // Token: 0x04003946 RID: 14662
        UpdateReSpecPoints,
        // Token: 0x04003947 RID: 14663
        UpdateCurrency,
        // Token: 0x04003948 RID: 14664
        UpdateFactionStanding,
        // Token: 0x04003949 RID: 14665
        Respawn,
        // Token: 0x0400394A RID: 14666
        ServerDebugLog,
        // Token: 0x0400394B RID: 14667
        CharacterEquipmentChanged,
        // Token: 0x0400394C RID: 14668
        RegenerationHealthChanged,
        // Token: 0x0400394D RID: 14669
        RegenerationEnergyChanged,
        // Token: 0x0400394E RID: 14670
        RegenerationMountHealthChanged,
        // Token: 0x0400394F RID: 14671
        RegenerationCraftingChanged,
        // Token: 0x04003950 RID: 14672
        RegenerationHealthEnergyComboChanged,
        // Token: 0x04003951 RID: 14673
        RegenerationPlayerComboChanged,
        // Token: 0x04003952 RID: 14674
        DurabilityChanged,
        // Token: 0x04003953 RID: 14675
        NewLoot,
        // Token: 0x04003954 RID: 14676
        AttachItemContainer,
        // Token: 0x04003955 RID: 14677
        DetachItemContainer,
        // Token: 0x04003956 RID: 14678
        InvalidateItemContainer,
        // Token: 0x04003957 RID: 14679
        LockItemContainer,
        // Token: 0x04003958 RID: 14680
        GuildUpdate,
        // Token: 0x04003959 RID: 14681
        GuildPlayerUpdated,
        // Token: 0x0400395A RID: 14682
        InvitedToGuild,
        // Token: 0x0400395B RID: 14683
        GuildMemberWorldUpdate,
        // Token: 0x0400395C RID: 14684
        UpdateMatchDetails,
        // Token: 0x0400395D RID: 14685
        ObjectEvent,
        // Token: 0x0400395E RID: 14686
        NewMonolithObject,
        // Token: 0x0400395F RID: 14687
        NewSiegeCampObject,
        // Token: 0x04003960 RID: 14688
        NewOrbObject,
        // Token: 0x04003961 RID: 14689
        NewCastleObject,
        // Token: 0x04003962 RID: 14690
        NewSpellEffectArea,
        // Token: 0x04003963 RID: 14691
        NewChainSpell,
        // Token: 0x04003964 RID: 14692
        UpdateChainSpell,
        // Token: 0x04003965 RID: 14693
        NewTreasureChest,
        // Token: 0x04003966 RID: 14694
        StartMatch,
        // Token: 0x04003967 RID: 14695
        StartTerritoryMatchInfos,
        // Token: 0x04003968 RID: 14696
        StartArenaMatchInfos,
        // Token: 0x04003969 RID: 14697
        EndTerritoryMatch,
        // Token: 0x0400396A RID: 14698
        EndArenaMatch,
        // Token: 0x0400396B RID: 14699
        MatchUpdate,
        // Token: 0x0400396C RID: 14700
        ActiveMatchUpdate,
        // Token: 0x0400396D RID: 14701
        NewMob,
        // Token: 0x0400396E RID: 14702
        DebugAggroInfo,
        // Token: 0x0400396F RID: 14703
        DebugVariablesInfo,
        // Token: 0x04003970 RID: 14704
        DebugReputationInfo,
        // Token: 0x04003971 RID: 14705
        DebugDiminishingReturnInfo,
        // Token: 0x04003972 RID: 14706
        DebugSmartClusterQueueInfo,
        // Token: 0x04003973 RID: 14707
        ClaimOrbStart,
        // Token: 0x04003974 RID: 14708
        ClaimOrbFinished,
        // Token: 0x04003975 RID: 14709
        ClaimOrbCancel,
        // Token: 0x04003976 RID: 14710
        OrbUpdate,
        // Token: 0x04003977 RID: 14711
        OrbClaimed,
        // Token: 0x04003978 RID: 14712
        NewWarCampObject,
        // Token: 0x04003979 RID: 14713
        NewMatchLootChestObject,
        // Token: 0x0400397A RID: 14714
        NewArenaExit,
        // Token: 0x0400397B RID: 14715
        GuildMemberTerritoryUpdate,
        // Token: 0x0400397C RID: 14716
        InvitedMercenaryToMatch,
        // Token: 0x0400397D RID: 14717
        ClusterInfoUpdate,
        // Token: 0x0400397E RID: 14718
        ForcedMovement,
        // Token: 0x0400397F RID: 14719
        ForcedMovementCancel,
        // Token: 0x04003980 RID: 14720
        CharacterStats,
        // Token: 0x04003981 RID: 14721
        CharacterStatsKillHistory,
        // Token: 0x04003982 RID: 14722
        CharacterStatsDeathHistory,
        // Token: 0x04003983 RID: 14723
        GuildStats,
        // Token: 0x04003984 RID: 14724
        KillHistoryDetails,
        // Token: 0x04003985 RID: 14725
        FullAchievementInfo,
        // Token: 0x04003986 RID: 14726
        FinishedAchievement,
        // Token: 0x04003987 RID: 14727
        AchievementProgressInfo,
        // Token: 0x04003988 RID: 14728
        FullAchievementProgressInfo,
        // Token: 0x04003989 RID: 14729
        FullTrackedAchievementInfo,
        // Token: 0x0400398A RID: 14730
        FullAutoLearnAchievementInfo,
        // Token: 0x0400398B RID: 14731
        QuestGiverQuestOffered,
        // Token: 0x0400398C RID: 14732
        QuestGiverDebugInfo,
        // Token: 0x0400398D RID: 14733
        ConsoleEvent,
        // Token: 0x0400398E RID: 14734
        TimeSync,
        // Token: 0x0400398F RID: 14735
        ChangeAvatar,
        // Token: 0x04003990 RID: 14736
        ChangeMountSkin,
        // Token: 0x04003991 RID: 14737
        GameEvent,
        // Token: 0x04003992 RID: 14738
        KilledPlayer,
        // Token: 0x04003993 RID: 14739
        Died,
        // Token: 0x04003994 RID: 14740
        KnockedDown,
        // Token: 0x04003995 RID: 14741
        MatchPlayerJoinedEvent,
        // Token: 0x04003996 RID: 14742
        MatchPlayerStatsEvent,
        // Token: 0x04003997 RID: 14743
        MatchPlayerStatsCompleteEvent,
        // Token: 0x04003998 RID: 14744
        MatchTimeLineEventEvent,
        // Token: 0x04003999 RID: 14745
        MatchPlayerMainGearStatsEvent,
        // Token: 0x0400399A RID: 14746
        MatchPlayerChangedAvatarEvent,
        // Token: 0x0400399B RID: 14747
        InvitationPlayerTrade,
        // Token: 0x0400399C RID: 14748
        PlayerTradeStart,
        // Token: 0x0400399D RID: 14749
        PlayerTradeCancel,
        // Token: 0x0400399E RID: 14750
        PlayerTradeUpdate,
        // Token: 0x0400399F RID: 14751
        PlayerTradeFinished,
        // Token: 0x040039A0 RID: 14752
        PlayerTradeAcceptChange,
        // Token: 0x040039A1 RID: 14753
        MiniMapPing,
        // Token: 0x040039A2 RID: 14754
        MarketPlaceNotification,
        // Token: 0x040039A3 RID: 14755
        DuellingChallengePlayer,
        // Token: 0x040039A4 RID: 14756
        NewDuellingPost,
        // Token: 0x040039A5 RID: 14757
        DuelStarted,
        // Token: 0x040039A6 RID: 14758
        DuelEnded,
        // Token: 0x040039A7 RID: 14759
        DuelDenied,
        // Token: 0x040039A8 RID: 14760
        DuelLeftArea,
        // Token: 0x040039A9 RID: 14761
        DuelReEnteredArea,
        // Token: 0x040039AA RID: 14762
        NewRealEstate,
        // Token: 0x040039AB RID: 14763
        MiniMapOwnedBuildingsPositions,
        // Token: 0x040039AC RID: 14764
        RealEstateListUpdate,
        // Token: 0x040039AD RID: 14765
        GuildLogoUpdate,
        // Token: 0x040039AE RID: 14766
        GuildLogoChanged,
        // Token: 0x040039AF RID: 14767
        PlaceableObjectPlace,
        // Token: 0x040039B0 RID: 14768
        PlaceableObjectPlaceCancel,
        // Token: 0x040039B1 RID: 14769
        FurnitureObjectBuffProviderInfo,
        // Token: 0x040039B2 RID: 14770
        FurnitureObjectCheatProviderInfo,
        // Token: 0x040039B3 RID: 14771
        FarmableObjectInfo,
        // Token: 0x040039B4 RID: 14772
        NewUnreadMails,
        // Token: 0x040039B5 RID: 14773
        GuildLogoObjectUpdate,
        // Token: 0x040039B6 RID: 14774
        StartLogout,
        // Token: 0x040039B7 RID: 14775
        NewChatChannels,
        // Token: 0x040039B8 RID: 14776
        JoinedChatChannel,
        // Token: 0x040039B9 RID: 14777
        LeftChatChannel,
        // Token: 0x040039BA RID: 14778
        RemovedChatChannel,
        // Token: 0x040039BB RID: 14779
        AccessStatus,
        // Token: 0x040039BC RID: 14780
        Mounted,
        // Token: 0x040039BD RID: 14781
        MountStart,
        // Token: 0x040039BE RID: 14782
        MountCancel,
        // Token: 0x040039BF RID: 14783
        NewTravelpoint,
        // Token: 0x040039C0 RID: 14784
        NewIslandAccessPoint,
        // Token: 0x040039C1 RID: 14785
        NewExit,
        // Token: 0x040039C2 RID: 14786
        UpdateHome,
        // Token: 0x040039C3 RID: 14787
        UpdateChatSettings,
        // Token: 0x040039C4 RID: 14788
        ResurrectionOffer,
        // Token: 0x040039C5 RID: 14789
        ResurrectionReply,
        // Token: 0x040039C6 RID: 14790
        LootEquipmentChanged,
        // Token: 0x040039C7 RID: 14791
        UpdateUnlockedGuildLogos,
        // Token: 0x040039C8 RID: 14792
        UpdateUnlockedAvatars,
        // Token: 0x040039C9 RID: 14793
        UpdateUnlockedAvatarRings,
        // Token: 0x040039CA RID: 14794
        UpdateUnlockedBuildings,
        // Token: 0x040039CB RID: 14795
        NewIslandManagement,
        // Token: 0x040039CC RID: 14796
        NewTeleportStone,
        // Token: 0x040039CD RID: 14797
        Cloak,
        // Token: 0x040039CE RID: 14798
        PartyInvitation,
        // Token: 0x040039CF RID: 14799
        PartyJoined,
        // Token: 0x040039D0 RID: 14800
        PartyDisbanded,
        // Token: 0x040039D1 RID: 14801
        PartyPlayerJoined,
        // Token: 0x040039D2 RID: 14802
        PartyChangedOrder,
        // Token: 0x040039D3 RID: 14803
        PartyPlayerLeft,
        // Token: 0x040039D4 RID: 14804
        PartyLeaderChanged,
        // Token: 0x040039D5 RID: 14805
        PartyLootSettingChangedPlayer,
        // Token: 0x040039D6 RID: 14806
        PartySilverGained,
        // Token: 0x040039D7 RID: 14807
        PartyPlayerUpdated,
        // Token: 0x040039D8 RID: 14808
        PartyInvitationPlayerBusy,
        // Token: 0x040039D9 RID: 14809
        PartyMarkedObjectsUpdated,
        // Token: 0x040039DA RID: 14810
        PartyOnClusterPartyJoined,
        // Token: 0x040039DB RID: 14811
        PartySetRoleFlag,
        // Token: 0x040039DC RID: 14812
        SpellCooldownUpdate,
        // Token: 0x040039DD RID: 14813
        NewHellgate,
        // Token: 0x040039DE RID: 14814
        NewHellgateExit,
        // Token: 0x040039DF RID: 14815
        NewExpeditionExit,
        // Token: 0x040039E0 RID: 14816
        NewExpeditionNarrator,
        // Token: 0x040039E1 RID: 14817
        ExitEnterStart,
        // Token: 0x040039E2 RID: 14818
        ExitEnterCancel,
        // Token: 0x040039E3 RID: 14819
        ExitEnterFinished,
        // Token: 0x040039E4 RID: 14820
        HellClusterTimeUpdate,
        // Token: 0x040039E5 RID: 14821
        NewQuestGiverObject,
        // Token: 0x040039E6 RID: 14822
        FullQuestInfo,
        // Token: 0x040039E7 RID: 14823
        QuestProgressInfo,
        // Token: 0x040039E8 RID: 14824
        QuestGiverInfoForPlayer,
        // Token: 0x040039E9 RID: 14825
        FullExpeditionInfo,
        // Token: 0x040039EA RID: 14826
        ExpeditionQuestProgressInfo,
        // Token: 0x040039EB RID: 14827
        InvitedToExpedition,
        // Token: 0x040039EC RID: 14828
        ExpeditionRegistrationInfo,
        // Token: 0x040039ED RID: 14829
        EnteringExpeditionStart,
        // Token: 0x040039EE RID: 14830
        EnteringExpeditionCancel,
        // Token: 0x040039EF RID: 14831
        RewardGranted,
        // Token: 0x040039F0 RID: 14832
        ArenaRegistrationInfo,
        // Token: 0x040039F1 RID: 14833
        EnteringArenaStart,
        // Token: 0x040039F2 RID: 14834
        EnteringArenaCancel,
        // Token: 0x040039F3 RID: 14835
        EnteringArenaLockStart,
        // Token: 0x040039F4 RID: 14836
        EnteringArenaLockCancel,
        // Token: 0x040039F5 RID: 14837
        InvitedToArenaMatch,
        // Token: 0x040039F6 RID: 14838
        PlayerCounts,
        // Token: 0x040039F7 RID: 14839
        InCombatStateUpdate,
        // Token: 0x040039F8 RID: 14840
        OtherGrabbedLoot,
        // Token: 0x040039F9 RID: 14841
        SiegeCampClaimStart,
        // Token: 0x040039FA RID: 14842
        SiegeCampClaimCancel,
        // Token: 0x040039FB RID: 14843
        SiegeCampClaimFinished,
        // Token: 0x040039FC RID: 14844
        SiegeCampScheduleResult,
        // Token: 0x040039FD RID: 14845
        TreasureChestUsingStart,
        // Token: 0x040039FE RID: 14846
        TreasureChestUsingFinished,
        // Token: 0x040039FF RID: 14847
        TreasureChestUsingCancel,
        // Token: 0x04003A00 RID: 14848
        TreasureChestUsingOpeningComplete,
        // Token: 0x04003A01 RID: 14849
        TreasureChestForceCloseInventory,
        // Token: 0x04003A02 RID: 14850
        PremiumChanged,
        // Token: 0x04003A03 RID: 14851
        PremiumExtended,
        // Token: 0x04003A04 RID: 14852
        PremiumLifeTimeRewardGained,
        // Token: 0x04003A05 RID: 14853
        LaborerGotUpgraded,
        // Token: 0x04003A06 RID: 14854
        JournalGotFull,
        // Token: 0x04003A07 RID: 14855
        JournalFillError,
        // Token: 0x04003A08 RID: 14856
        FriendRequest,
        // Token: 0x04003A09 RID: 14857
        FriendRequestInfos,
        // Token: 0x04003A0A RID: 14858
        FriendInfos,
        // Token: 0x04003A0B RID: 14859
        FriendRequestAnswered,
        // Token: 0x04003A0C RID: 14860
        FriendOnlineStatus,
        // Token: 0x04003A0D RID: 14861
        FriendRequestCanceled,
        // Token: 0x04003A0E RID: 14862
        FriendRemoved,
        // Token: 0x04003A0F RID: 14863
        FriendUpdated,
        // Token: 0x04003A10 RID: 14864
        PartyLootItems,
        // Token: 0x04003A11 RID: 14865
        PartyLootItemsRemoved,
        // Token: 0x04003A12 RID: 14866
        ReputationUpdate,
        // Token: 0x04003A13 RID: 14867
        DefenseUnitAttackBegin,
        // Token: 0x04003A14 RID: 14868
        DefenseUnitAttackEnd,
        // Token: 0x04003A15 RID: 14869
        DefenseUnitAttackDamage,
        // Token: 0x04003A16 RID: 14870
        UnrestrictedPvpZoneUpdate,
        // Token: 0x04003A17 RID: 14871
        ReputationImplicationUpdate,
        // Token: 0x04003A18 RID: 14872
        NewMountObject,
        // Token: 0x04003A19 RID: 14873
        MountHealthUpdate,
        // Token: 0x04003A1A RID: 14874
        MountCooldownUpdate,
        // Token: 0x04003A1B RID: 14875
        NewExpeditionAgent,
        // Token: 0x04003A1C RID: 14876
        NewExpeditionCheckPoint,
        // Token: 0x04003A1D RID: 14877
        ExpeditionStartEvent,
        // Token: 0x04003A1E RID: 14878
        VoteEvent,
        // Token: 0x04003A1F RID: 14879
        RatingEvent,
        // Token: 0x04003A20 RID: 14880
        NewArenaAgent,
        // Token: 0x04003A21 RID: 14881
        BoostFarmable,
        // Token: 0x04003A22 RID: 14882
        UseFunction,
        // Token: 0x04003A23 RID: 14883
        NewPortalEntrance,
        // Token: 0x04003A24 RID: 14884
        NewPortalExit,
        // Token: 0x04003A25 RID: 14885
        NewRandomDungeonExit,
        // Token: 0x04003A26 RID: 14886
        WaitingQueueUpdate,
        // Token: 0x04003A27 RID: 14887
        PlayerMovementRateUpdate,
        // Token: 0x04003A28 RID: 14888
        ObserveStart,
        // Token: 0x04003A29 RID: 14889
        MinimapZergs,
        // Token: 0x04003A2A RID: 14890
        PaymentTransactions,
        // Token: 0x04003A2B RID: 14891
        PerformanceStatsUpdate,
        // Token: 0x04003A2C RID: 14892
        OverloadModeUpdate,
        // Token: 0x04003A2D RID: 14893
        DebugDrawEvent,
        // Token: 0x04003A2E RID: 14894
        RecordCameraMove,
        // Token: 0x04003A2F RID: 14895
        RecordStart,
        // Token: 0x04003A30 RID: 14896
        TerritoryClaimStart,
        // Token: 0x04003A31 RID: 14897
        TerritoryClaimCancel,
        // Token: 0x04003A32 RID: 14898
        TerritoryClaimFinished,
        // Token: 0x04003A33 RID: 14899
        TerritoryScheduleResult,
        // Token: 0x04003A34 RID: 14900
        UpdateAccountState,
        // Token: 0x04003A35 RID: 14901
        StartDeterministicRoam,
        // Token: 0x04003A36 RID: 14902
        GuildFullAccessTagsUpdated,
        // Token: 0x04003A37 RID: 14903
        GuildAccessTagUpdated,
        // Token: 0x04003A38 RID: 14904
        GvgSeasonUpdate,
        // Token: 0x04003A39 RID: 14905
        GvgSeasonCheatCommand,
        // Token: 0x04003A3A RID: 14906
        SeasonPointsByKillingBooster,
        // Token: 0x04003A3B RID: 14907
        FishingStart,
        // Token: 0x04003A3C RID: 14908
        FishingCast,
        // Token: 0x04003A3D RID: 14909
        FishingCatch,
        // Token: 0x04003A3E RID: 14910
        FishingFinished,
        // Token: 0x04003A3F RID: 14911
        FishingCancel,
        // Token: 0x04003A40 RID: 14912
        NewFloatObject,
        // Token: 0x04003A41 RID: 14913
        NewFishingZoneObject,
        // Token: 0x04003A42 RID: 14914
        FishingMiniGame,
        // Token: 0x04003A43 RID: 14915
        SteamAchievementCompleted,
        // Token: 0x04003A44 RID: 14916
        UpdatePuppet,
        // Token: 0x04003A45 RID: 14917
        ChangeFlaggingFinished,
        // Token: 0x04003A46 RID: 14918
        NewOutpostObject,
        // Token: 0x04003A47 RID: 14919
        OutpostUpdate,
        // Token: 0x04003A48 RID: 14920
        OutpostClaimed,
        // Token: 0x04003A49 RID: 14921
        OutpostReward,
        // Token: 0x04003A4A RID: 14922
        OverChargeEnd,
        // Token: 0x04003A4B RID: 14923
        OverChargeStatus,
        // Token: 0x04003A4C RID: 14924
        PartyFinderFullUpdate,
        // Token: 0x04003A4D RID: 14925
        PartyFinderUpdate,
        // Token: 0x04003A4E RID: 14926
        PartyFinderApplicantsUpdate,
        // Token: 0x04003A4F RID: 14927
        PartyFinderEquipmentSnapshot,
        // Token: 0x04003A50 RID: 14928
        PartyFinderJoinRequestDeclined,
        // Token: 0x04003A51 RID: 14929
        NewUnlockedPersonalSeasonRewards,
        // Token: 0x04003A52 RID: 14930
        PersonalSeasonPointsGained,
        // Token: 0x04003A53 RID: 14931
        EasyAntiCheatMessageToClient,
        // Token: 0x04003A54 RID: 14932
        MatchLootChestOpeningStart,
        // Token: 0x04003A55 RID: 14933
        MatchLootChestOpeningFinished,
        // Token: 0x04003A56 RID: 14934
        MatchLootChestOpeningCancel,
        // Token: 0x04003A57 RID: 14935
        NotifyCrystalMatchReward,
        // Token: 0x04003A58 RID: 14936
        CrystalRealmFeedback,
        // Token: 0x04003A59 RID: 14937
        NewLocationMarker,
        // Token: 0x04003A5A RID: 14938
        NewTutorialBlocker,
        // Token: 0x04003A5B RID: 14939
        NewTileSwitch,
        // Token: 0x04003A5C RID: 14940
        NewInformationProvider,
        // Token: 0x04003A5D RID: 14941
        NewDynamicGuildLogo,
        // Token: 0x04003A5E RID: 14942
        TutorialUpdate,
        // Token: 0x04003A5F RID: 14943
        TriggerHintBox,
        // Token: 0x04003A60 RID: 14944
        RandomDungeonPositionInfo,
        // Token: 0x04003A61 RID: 14945
        NewLootChest,
        // Token: 0x04003A62 RID: 14946
        UpdateLootChest,
        // Token: 0x04003A63 RID: 14947
        LootChestOpened,
        // Token: 0x04003A64 RID: 14948
        NewShrine,
        // Token: 0x04003A65 RID: 14949
        UpdateShrine,
        // Token: 0x04003A66 RID: 14950
        MutePlayerUpdate,
        // Token: 0x04003A67 RID: 14951
        ShopTileUpdate,
        // Token: 0x04003A68 RID: 14952
        ShopUpdate,
        // Token: 0x04003A69 RID: 14953
        EasyAntiCheatKick,
        // Token: 0x04003A6A RID: 14954
        UnlockVanityUnlock,
        // Token: 0x04003A6B RID: 14955
        AvatarUnlocked,
        // Token: 0x04003A6C RID: 14956
        CustomizationChanged,
        // Token: 0x04003A6D RID: 14957
        BaseVaultInfo,
        // Token: 0x04003A6E RID: 14958
        GuildVaultInfo,
        // Token: 0x04003A6F RID: 14959
        BankVaultInfo,
        // Token: 0x04003A70 RID: 14960
        RecoveryVaultPlayerInfo,
        // Token: 0x04003A71 RID: 14961
        RecoveryVaultGuildInfo,
        // Token: 0x04003A72 RID: 14962
        UpdateWardrobe,
        // Token: 0x04003A73 RID: 14963
        CastlePhaseChanged,
        // Token: 0x04003A74 RID: 14964
        GuildAccountLogEvent,
        // Token: 0x04003A75 RID: 14965
        NewHideoutObject,
        // Token: 0x04003A76 RID: 14966
        NewHideoutManagement,
        // Token: 0x04003A77 RID: 14967
        NewHideoutExit,
        // Token: 0x04003A78 RID: 14968
        InitHideoutAttackStart,
        // Token: 0x04003A79 RID: 14969
        InitHideoutAttackCancel,
        // Token: 0x04003A7A RID: 14970
        InitHideoutAttackFinished,
        // Token: 0x04003A7B RID: 14971
        HideoutManagementUpdate,
        // Token: 0x04003A7C RID: 14972
        IpChanged,
        // Token: 0x04003A7D RID: 14973
        SmartClusterQueueUpdateInfo,
        // Token: 0x04003A7E RID: 14974
        SmartClusterQueueActiveInfo,
        // Token: 0x04003A7F RID: 14975
        SmartClusterQueueKickWarning,
        // Token: 0x04003A80 RID: 14976
        SmartClusterQueueInvite,
        // Token: 0x04003A81 RID: 14977
        ReceivedGvgSeasonPoints,
        // Token: 0x04003A82 RID: 14978
        TerritoryBonusLevelUpdate,
        // Token: 0x04003A83 RID: 14979
        OpenWorldAttackScheduleStart,
        // Token: 0x04003A84 RID: 14980
        OpenWorldAttackScheduleFinished,
        // Token: 0x04003A85 RID: 14981
        OpenWorldAttackScheduleCancel,
        // Token: 0x04003A86 RID: 14982
        OpenWorldAttackConquerStart,
        // Token: 0x04003A87 RID: 14983
        OpenWorldAttackConquerFinished,
        // Token: 0x04003A88 RID: 14984
        OpenWorldAttackConquerCancel,
        // Token: 0x04003A89 RID: 14985
        OpenWorldAttackConquerStatus,
        // Token: 0x04003A8A RID: 14986
        OpenWorldAttackStart,
        // Token: 0x04003A8B RID: 14987
        OpenWorldAttackEnd,
        // Token: 0x04003A8C RID: 14988
        NewRandomResourceBlocker,
        // Token: 0x04003A8D RID: 14989
        NewHomeObject,
        // Token: 0x04003A8E RID: 14990
        HideoutObjectUpdate
    }
}
