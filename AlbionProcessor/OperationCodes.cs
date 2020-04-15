using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionProcessor
{
    public enum OperationCodes : short
    {
        // Token: 0x04003A90 RID: 14992
        Unused,
        // Token: 0x04003A91 RID: 14993
        Ping,
        // Token: 0x04003A92 RID: 14994
        Join,
        // Token: 0x04003A93 RID: 14995
        CreateAccount,
        // Token: 0x04003A94 RID: 14996
        Login,
        // Token: 0x04003A95 RID: 14997
        SendCrashLog,
        // Token: 0x04003A96 RID: 14998
        SendTraceRoute,
        // Token: 0x04003A97 RID: 14999
        SendGamePingInfo,
        // Token: 0x04003A98 RID: 15000
        CreateCharacter,
        // Token: 0x04003A99 RID: 15001
        DeleteCharacter,
        // Token: 0x04003A9A RID: 15002
        SelectCharacter,
        // Token: 0x04003A9B RID: 15003
        RedeemKeycode,
        // Token: 0x04003A9C RID: 15004
        GetGameServerByCluster,
        // Token: 0x04003A9D RID: 15005
        GetActiveSubscription,
        // Token: 0x04003A9E RID: 15006
        GetShopPurchaseUrl,
        // Token: 0x04003A9F RID: 15007
        GetBuyTrialDetails,
        // Token: 0x04003AA0 RID: 15008
        GetReferralSeasonDetails,
        // Token: 0x04003AA1 RID: 15009
        GetReferralLink,
        // Token: 0x04003AA2 RID: 15010
        GetAvailableTrialKeys,
        // Token: 0x04003AA3 RID: 15011
        GetShopTilesForCategory,
        // Token: 0x04003AA4 RID: 15012
        Move,
        // Token: 0x04003AA5 RID: 15013
        AttackStart,
        // Token: 0x04003AA6 RID: 15014
        CastStart,
        // Token: 0x04003AA7 RID: 15015
        CastCancel,
        // Token: 0x04003AA8 RID: 15016
        TerminateToggleSpell,
        // Token: 0x04003AA9 RID: 15017
        ChannelingCancel,
        // Token: 0x04003AAA RID: 15018
        AttackBuildingStart,
        // Token: 0x04003AAB RID: 15019
        InventoryDestroyItem,
        // Token: 0x04003AAC RID: 15020
        InventoryMoveItem,
        // Token: 0x04003AAD RID: 15021
        InventoryRecoverItem,
        // Token: 0x04003AAE RID: 15022
        InventoryRecoverAllItems,
        // Token: 0x04003AAF RID: 15023
        InventorySplitStack,
        // Token: 0x04003AB0 RID: 15024
        InventorySplitStackInto,
        // Token: 0x04003AB1 RID: 15025
        GetClusterData,
        // Token: 0x04003AB2 RID: 15026
        ChangeCluster,
        // Token: 0x04003AB3 RID: 15027
        ConsoleCommand,
        // Token: 0x04003AB4 RID: 15028
        ChatMessage,
        // Token: 0x04003AB5 RID: 15029
        ReportClientError,
        // Token: 0x04003AB6 RID: 15030
        RegisterToObject,
        // Token: 0x04003AB7 RID: 15031
        UnRegisterFromObject,
        // Token: 0x04003AB8 RID: 15032
        CraftBuildingChangeSettings,
        // Token: 0x04003AB9 RID: 15033
        CraftBuildingTakeMoney,
        // Token: 0x04003ABA RID: 15034
        RepairBuildingChangeSettings,
        // Token: 0x04003ABB RID: 15035
        RepairBuildingTakeMoney,
        // Token: 0x04003ABC RID: 15036
        ActionBuildingChangeSettings,
        // Token: 0x04003ABD RID: 15037
        HarvestStart,
        // Token: 0x04003ABE RID: 15038
        HarvestCancel,
        // Token: 0x04003ABF RID: 15039
        TakeSilver,
        // Token: 0x04003AC0 RID: 15040
        ActionOnBuildingStart,
        // Token: 0x04003AC1 RID: 15041
        ActionOnBuildingCancel,
        // Token: 0x04003AC2 RID: 15042
        ItemRerollQualityStart,
        // Token: 0x04003AC3 RID: 15043
        ItemRerollQualityCancel,
        // Token: 0x04003AC4 RID: 15044
        InstallResourceStart,
        // Token: 0x04003AC5 RID: 15045
        InstallResourceCancel,
        // Token: 0x04003AC6 RID: 15046
        InstallSilver,
        // Token: 0x04003AC7 RID: 15047
        BuildingFillNutrition,
        // Token: 0x04003AC8 RID: 15048
        BuildingChangeRenovationState,
        // Token: 0x04003AC9 RID: 15049
        BuildingBuySkin,
        // Token: 0x04003ACA RID: 15050
        BuildingClaim,
        // Token: 0x04003ACB RID: 15051
        BuildingGiveup,
        // Token: 0x04003ACC RID: 15052
        BuildingNutritionSilverStorageDeposit,
        // Token: 0x04003ACD RID: 15053
        BuildingNutritionSilverStorageWithdraw,
        // Token: 0x04003ACE RID: 15054
        BuildingNutritionSilverRewardSet,
        // Token: 0x04003ACF RID: 15055
        ConstructionSiteCreate,
        // Token: 0x04003AD0 RID: 15056
        PlaceableObjectPlace,
        // Token: 0x04003AD1 RID: 15057
        PlaceableObjectPlaceCancel,
        // Token: 0x04003AD2 RID: 15058
        PlaceableObjectPickup,
        // Token: 0x04003AD3 RID: 15059
        FurnitureObjectUse,
        // Token: 0x04003AD4 RID: 15060
        FarmableHarvest,
        // Token: 0x04003AD5 RID: 15061
        FarmableFinishGrownItem,
        // Token: 0x04003AD6 RID: 15062
        FarmableDestroy,
        // Token: 0x04003AD7 RID: 15063
        FarmableGetProduct,
        // Token: 0x04003AD8 RID: 15064
        FarmableFill,
        // Token: 0x04003AD9 RID: 15065
        TearDownConstructionSite,
        // Token: 0x04003ADA RID: 15066
        CastleGateUse,
        // Token: 0x04003ADB RID: 15067
        AuctionCreateOffer,
        // Token: 0x04003ADC RID: 15068
        AuctionCreateRequest,
        // Token: 0x04003ADD RID: 15069
        AuctionGetOffers,
        // Token: 0x04003ADE RID: 15070
        AuctionGetRequests,
        // Token: 0x04003ADF RID: 15071
        AuctionBuyOffer,
        // Token: 0x04003AE0 RID: 15072
        AuctionAbortAuction,
        // Token: 0x04003AE1 RID: 15073
        AuctionModifyAuction,
        // Token: 0x04003AE2 RID: 15074
        AuctionAbortOffer,
        // Token: 0x04003AE3 RID: 15075
        AuctionAbortRequest,
        // Token: 0x04003AE4 RID: 15076
        AuctionSellRequest,
        // Token: 0x04003AE5 RID: 15077
        AuctionGetFinishedAuctions,
        // Token: 0x04003AE6 RID: 15078
        AuctionFetchAuction,
        // Token: 0x04003AE7 RID: 15079
        AuctionGetMyOpenOffers,
        // Token: 0x04003AE8 RID: 15080
        AuctionGetMyOpenRequests,
        // Token: 0x04003AE9 RID: 15081
        AuctionGetMyOpenAuctions,
        // Token: 0x04003AEA RID: 15082
        AuctionGetItemsAverage,
        // Token: 0x04003AEB RID: 15083
        AuctionGetItemAverageStats,
        // Token: 0x04003AEC RID: 15084
        AuctionGetItemAverageValue,
        // Token: 0x04003AED RID: 15085
        ContainerOpen,
        // Token: 0x04003AEE RID: 15086
        ContainerClose,
        // Token: 0x04003AEF RID: 15087
        ContainerManageSubContainer,
        // Token: 0x04003AF0 RID: 15088
        Respawn,
        // Token: 0x04003AF1 RID: 15089
        Suicide,
        // Token: 0x04003AF2 RID: 15090
        JoinGuild,
        // Token: 0x04003AF3 RID: 15091
        LeaveGuild,
        // Token: 0x04003AF4 RID: 15092
        CreateGuild,
        // Token: 0x04003AF5 RID: 15093
        InviteToGuild,
        // Token: 0x04003AF6 RID: 15094
        DeclineGuildInvitation,
        // Token: 0x04003AF7 RID: 15095
        KickFromGuild,
        // Token: 0x04003AF8 RID: 15096
        DuellingChallengePlayer,
        // Token: 0x04003AF9 RID: 15097
        DuellingAcceptChallenge,
        // Token: 0x04003AFA RID: 15098
        DuellingDenyChallenge,
        // Token: 0x04003AFB RID: 15099
        ChangeClusterTax,
        // Token: 0x04003AFC RID: 15100
        ClaimTerritory,
        // Token: 0x04003AFD RID: 15101
        GiveUpTerritory,
        // Token: 0x04003AFE RID: 15102
        ChangeTerritoryAccessRights,
        // Token: 0x04003AFF RID: 15103
        GetMonolithInfo,
        // Token: 0x04003B00 RID: 15104
        GetClaimInfo,
        // Token: 0x04003B01 RID: 15105
        GetAttackInfo,
        // Token: 0x04003B02 RID: 15106
        GetTerritorySeasonPoints,
        // Token: 0x04003B03 RID: 15107
        GetAttackSchedule,
        // Token: 0x04003B04 RID: 15108
        ScheduleAttack,
        // Token: 0x04003B05 RID: 15109
        GetMatches,
        // Token: 0x04003B06 RID: 15110
        GetMatchDetails,
        // Token: 0x04003B07 RID: 15111
        JoinMatch,
        // Token: 0x04003B08 RID: 15112
        LeaveMatch,
        // Token: 0x04003B09 RID: 15113
        ChangeChatSettings,
        // Token: 0x04003B0A RID: 15114
        LogoutStart,
        // Token: 0x04003B0B RID: 15115
        LogoutCancel,
        // Token: 0x04003B0C RID: 15116
        ClaimOrbStart,
        // Token: 0x04003B0D RID: 15117
        ClaimOrbCancel,
        // Token: 0x04003B0E RID: 15118
        MatchLootChestOpeningStart,
        // Token: 0x04003B0F RID: 15119
        MatchLootChestOpeningCancel,
        // Token: 0x04003B10 RID: 15120
        DepositToGuildAccount,
        // Token: 0x04003B11 RID: 15121
        WithdrawalFromAccount,
        // Token: 0x04003B12 RID: 15122
        ChangeGuildPayUpkeepFlag,
        // Token: 0x04003B13 RID: 15123
        ChangeGuildTax,
        // Token: 0x04003B14 RID: 15124
        GetMyTerritories,
        // Token: 0x04003B15 RID: 15125
        MorganaCommand,
        // Token: 0x04003B16 RID: 15126
        GetServerInfo,
        // Token: 0x04003B17 RID: 15127
        InviteMercenaryToMatch,
        // Token: 0x04003B18 RID: 15128
        SubscribeToCluster,
        // Token: 0x04003B19 RID: 15129
        AnswerMercenaryInvitation,
        // Token: 0x04003B1A RID: 15130
        GetCharacterEquipment,
        // Token: 0x04003B1B RID: 15131
        GetCharacterSteamAchievements,
        // Token: 0x04003B1C RID: 15132
        GetCharacterStats,
        // Token: 0x04003B1D RID: 15133
        GetKillHistoryDetails,
        // Token: 0x04003B1E RID: 15134
        LearnMasteryLevel,
        // Token: 0x04003B1F RID: 15135
        ReSpecAchievement,
        // Token: 0x04003B20 RID: 15136
        ChangeAvatar,
        // Token: 0x04003B21 RID: 15137
        GetRankings,
        // Token: 0x04003B22 RID: 15138
        GetRank,
        // Token: 0x04003B23 RID: 15139
        GetGvgSeasonRankings,
        // Token: 0x04003B24 RID: 15140
        GetGvgSeasonRank,
        // Token: 0x04003B25 RID: 15141
        GetGvgSeasonHistoryRankings,
        // Token: 0x04003B26 RID: 15142
        GetGvgSeasonGuildMemberHistory,
        // Token: 0x04003B27 RID: 15143
        KickFromGvGMatch,
        // Token: 0x04003B28 RID: 15144
        GetChestLogs,
        // Token: 0x04003B29 RID: 15145
        GetAccessRightLogs,
        // Token: 0x04003B2A RID: 15146
        GetGuildAccountLogs,
        // Token: 0x04003B2B RID: 15147
        GetGuildAccountLogsLargeAmount,
        // Token: 0x04003B2C RID: 15148
        InviteToPlayerTrade,
        // Token: 0x04003B2D RID: 15149
        PlayerTradeCancel,
        // Token: 0x04003B2E RID: 15150
        PlayerTradeInvitationAccept,
        // Token: 0x04003B2F RID: 15151
        PlayerTradeAddItem,
        // Token: 0x04003B30 RID: 15152
        PlayerTradeRemoveItem,
        // Token: 0x04003B31 RID: 15153
        PlayerTradeAcceptTrade,
        // Token: 0x04003B32 RID: 15154
        PlayerTradeSetSilverOrGold,
        // Token: 0x04003B33 RID: 15155
        SendMiniMapPing,
        // Token: 0x04003B34 RID: 15156
        Stuck,
        // Token: 0x04003B35 RID: 15157
        BuyRealEstate,
        // Token: 0x04003B36 RID: 15158
        ClaimRealEstate,
        // Token: 0x04003B37 RID: 15159
        GiveUpRealEstate,
        // Token: 0x04003B38 RID: 15160
        ChangeRealEstateOutline,
        // Token: 0x04003B39 RID: 15161
        GetMailInfos,
        // Token: 0x04003B3A RID: 15162
        ReadMail,
        // Token: 0x04003B3B RID: 15163
        SendNewMail,
        // Token: 0x04003B3C RID: 15164
        DeleteMail,
        // Token: 0x04003B3D RID: 15165
        ClaimAttachmentFromMail,
        // Token: 0x04003B3E RID: 15166
        UpdateLfgInfo,
        // Token: 0x04003B3F RID: 15167
        GetLfgInfos,
        // Token: 0x04003B40 RID: 15168
        GetMyGuildLfgInfo,
        // Token: 0x04003B41 RID: 15169
        GetLfgDescriptionText,
        // Token: 0x04003B42 RID: 15170
        LfgApplyToGuild,
        // Token: 0x04003B43 RID: 15171
        AnswerLfgGuildApplication,
        // Token: 0x04003B44 RID: 15172
        RegisterChatPeer,
        // Token: 0x04003B45 RID: 15173
        SendChatMessage,
        // Token: 0x04003B46 RID: 15174
        JoinChatChannel,
        // Token: 0x04003B47 RID: 15175
        LeaveChatChannel,
        // Token: 0x04003B48 RID: 15176
        SendWhisperMessage,
        // Token: 0x04003B49 RID: 15177
        Say,
        // Token: 0x04003B4A RID: 15178
        PlayEmote,
        // Token: 0x04003B4B RID: 15179
        StopEmote,
        // Token: 0x04003B4C RID: 15180
        GetClusterMapInfo,
        // Token: 0x04003B4D RID: 15181
        AccessRightsChangeSettings,
        // Token: 0x04003B4E RID: 15182
        Mount,
        // Token: 0x04003B4F RID: 15183
        MountCancel,
        // Token: 0x04003B50 RID: 15184
        BuyJourney,
        // Token: 0x04003B51 RID: 15185
        SetSaleStatusForEstate,
        // Token: 0x04003B52 RID: 15186
        ResolveGuildOrPlayerName,
        // Token: 0x04003B53 RID: 15187
        GetRespawnInfos,
        // Token: 0x04003B54 RID: 15188
        MakeHome,
        // Token: 0x04003B55 RID: 15189
        LeaveHome,
        // Token: 0x04003B56 RID: 15190
        ResurrectionReply,
        // Token: 0x04003B57 RID: 15191
        AllianceCreate,
        // Token: 0x04003B58 RID: 15192
        AllianceDisband,
        // Token: 0x04003B59 RID: 15193
        AllianceGetMemberInfos,
        // Token: 0x04003B5A RID: 15194
        AllianceInvite,
        // Token: 0x04003B5B RID: 15195
        AllianceAnswerInvitation,
        // Token: 0x04003B5C RID: 15196
        AllianceCancelInvitation,
        // Token: 0x04003B5D RID: 15197
        AllianceKickGuild,
        // Token: 0x04003B5E RID: 15198
        AllianceLeave,
        // Token: 0x04003B5F RID: 15199
        AllianceChangeGoldPaymentFlag,
        // Token: 0x04003B60 RID: 15200
        AllianceGetDetailInfo,
        // Token: 0x04003B61 RID: 15201
        GetIslandInfos,
        // Token: 0x04003B62 RID: 15202
        AbandonMyIsland,
        // Token: 0x04003B63 RID: 15203
        BuyMyIsland,
        // Token: 0x04003B64 RID: 15204
        BuyGuildIsland,
        // Token: 0x04003B65 RID: 15205
        AbandonGuildIsland,
        // Token: 0x04003B66 RID: 15206
        UpgradeMyIsland,
        // Token: 0x04003B67 RID: 15207
        UpgradeGuildIsland,
        // Token: 0x04003B68 RID: 15208
        MoveMyIsland,
        // Token: 0x04003B69 RID: 15209
        MoveGuildIsland,
        // Token: 0x04003B6A RID: 15210
        TerritoryFillNutrition,
        // Token: 0x04003B6B RID: 15211
        TeleportBack,
        // Token: 0x04003B6C RID: 15212
        PartyInvitePlayer,
        // Token: 0x04003B6D RID: 15213
        PartyAnswerInvitation,
        // Token: 0x04003B6E RID: 15214
        PartyLeave,
        // Token: 0x04003B6F RID: 15215
        PartyKickPlayer,
        // Token: 0x04003B70 RID: 15216
        PartyMakeLeader,
        // Token: 0x04003B71 RID: 15217
        PartyChangeLootSetting,
        // Token: 0x04003B72 RID: 15218
        PartyMarkObject,
        // Token: 0x04003B73 RID: 15219
        PartySetRole,
        // Token: 0x04003B74 RID: 15220
        GetGuildMOTD,
        // Token: 0x04003B75 RID: 15221
        SetGuildMOTD,
        // Token: 0x04003B76 RID: 15222
        ExitEnterStart,
        // Token: 0x04003B77 RID: 15223
        ExitEnterCancel,
        // Token: 0x04003B78 RID: 15224
        QuestGiverRequest,
        // Token: 0x04003B79 RID: 15225
        GoldMarketGetBuyOffer,
        // Token: 0x04003B7A RID: 15226
        GoldMarketGetBuyOfferFromSilver,
        // Token: 0x04003B7B RID: 15227
        GoldMarketGetSellOffer,
        // Token: 0x04003B7C RID: 15228
        GoldMarketGetSellOfferFromSilver,
        // Token: 0x04003B7D RID: 15229
        GoldMarketBuyGold,
        // Token: 0x04003B7E RID: 15230
        GoldMarketSellGold,
        // Token: 0x04003B7F RID: 15231
        GoldMarketCreateSellOrder,
        // Token: 0x04003B80 RID: 15232
        GoldMarketCreateBuyOrder,
        // Token: 0x04003B81 RID: 15233
        GoldMarketGetInfos,
        // Token: 0x04003B82 RID: 15234
        GoldMarketCancelOrder,
        // Token: 0x04003B83 RID: 15235
        GoldMarketGetAverageInfo,
        // Token: 0x04003B84 RID: 15236
        SiegeCampClaimStart,
        // Token: 0x04003B85 RID: 15237
        SiegeCampClaimCancel,
        // Token: 0x04003B86 RID: 15238
        TreasureChestUsingStart,
        // Token: 0x04003B87 RID: 15239
        TreasureChestUsingCancel,
        // Token: 0x04003B88 RID: 15240
        UseLootChest,
        // Token: 0x04003B89 RID: 15241
        UseShrine,
        // Token: 0x04003B8A RID: 15242
        LaborerStartJob,
        // Token: 0x04003B8B RID: 15243
        LaborerTakeJobLoot,
        // Token: 0x04003B8C RID: 15244
        LaborerDismiss,
        // Token: 0x04003B8D RID: 15245
        LaborerMove,
        // Token: 0x04003B8E RID: 15246
        LaborerBuyItem,
        // Token: 0x04003B8F RID: 15247
        LaborerUpgrade,
        // Token: 0x04003B90 RID: 15248
        BuyPremium,
        // Token: 0x04003B91 RID: 15249
        BuyTrial,
        // Token: 0x04003B92 RID: 15250
        RealEstateGetAuctionData,
        // Token: 0x04003B93 RID: 15251
        RealEstateBidOnAuction,
        // Token: 0x04003B94 RID: 15252
        GetSiegeCampCooldown,
        // Token: 0x04003B95 RID: 15253
        FriendInvite,
        // Token: 0x04003B96 RID: 15254
        FriendAnswerInvitation,
        // Token: 0x04003B97 RID: 15255
        FriendCancelnvitation,
        // Token: 0x04003B98 RID: 15256
        FriendRemove,
        // Token: 0x04003B99 RID: 15257
        InventoryStack,
        // Token: 0x04003B9A RID: 15258
        InventorySort,
        // Token: 0x04003B9B RID: 15259
        EquipmentItemChangeSpell,
        // Token: 0x04003B9C RID: 15260
        ExpeditionRegister,
        // Token: 0x04003B9D RID: 15261
        ExpeditionRegisterCancel,
        // Token: 0x04003B9E RID: 15262
        JoinExpedition,
        // Token: 0x04003B9F RID: 15263
        DeclineExpeditionInvitation,
        // Token: 0x04003BA0 RID: 15264
        VoteStart,
        // Token: 0x04003BA1 RID: 15265
        VoteDoVote,
        // Token: 0x04003BA2 RID: 15266
        RatingDoRate,
        // Token: 0x04003BA3 RID: 15267
        EnteringExpeditionStart,
        // Token: 0x04003BA4 RID: 15268
        EnteringExpeditionCancel,
        // Token: 0x04003BA5 RID: 15269
        ActivateExpeditionCheckPoint,
        // Token: 0x04003BA6 RID: 15270
        ArenaRegister,
        // Token: 0x04003BA7 RID: 15271
        ArenaRegisterCancel,
        // Token: 0x04003BA8 RID: 15272
        ArenaLeave,
        // Token: 0x04003BA9 RID: 15273
        JoinArenaMatch,
        // Token: 0x04003BAA RID: 15274
        DeclineArenaInvitation,
        // Token: 0x04003BAB RID: 15275
        EnteringArenaStart,
        // Token: 0x04003BAC RID: 15276
        EnteringArenaCancel,
        // Token: 0x04003BAD RID: 15277
        ArenaCustomMatch,
        // Token: 0x04003BAE RID: 15278
        ArenaCustomMatchCreate,
        // Token: 0x04003BAF RID: 15279
        UpdateCharacterStatement,
        // Token: 0x04003BB0 RID: 15280
        BoostFarmable,
        // Token: 0x04003BB1 RID: 15281
        GetStrikeHistory,
        // Token: 0x04003BB2 RID: 15282
        UseFunction,
        // Token: 0x04003BB3 RID: 15283
        UsePortalEntrance,
        // Token: 0x04003BB4 RID: 15284
        ResetPortalBinding,
        // Token: 0x04003BB5 RID: 15285
        QueryPortalBinding,
        // Token: 0x04003BB6 RID: 15286
        ClaimPaymentTransaction,
        // Token: 0x04003BB7 RID: 15287
        ChangeUseFlag,
        // Token: 0x04003BB8 RID: 15288
        ClientPerformanceStats,
        // Token: 0x04003BB9 RID: 15289
        ExtendedHardwareStats,
        // Token: 0x04003BBA RID: 15290
        ClientLowMemoryWarning,
        // Token: 0x04003BBB RID: 15291
        TerritoryClaimStart,
        // Token: 0x04003BBC RID: 15292
        TerritoryClaimCancel,
        // Token: 0x04003BBD RID: 15293
        RequestAppStoreProducts,
        // Token: 0x04003BBE RID: 15294
        VerifyProductPurchase,
        // Token: 0x04003BBF RID: 15295
        QueryGuildPlayerStats,
        // Token: 0x04003BC0 RID: 15296
        QueryAllianceGuildStats,
        // Token: 0x04003BC1 RID: 15297
        TrackAchievements,
        // Token: 0x04003BC2 RID: 15298
        SetAchievementsAutoLearn,
        // Token: 0x04003BC3 RID: 15299
        DepositItemToGuildCurrency,
        // Token: 0x04003BC4 RID: 15300
        WithdrawalItemFromGuildCurrency,
        // Token: 0x04003BC5 RID: 15301
        AuctionSellSpecificItemRequest,
        // Token: 0x04003BC6 RID: 15302
        FishingStart,
        // Token: 0x04003BC7 RID: 15303
        FishingCasting,
        // Token: 0x04003BC8 RID: 15304
        FishingCast,
        // Token: 0x04003BC9 RID: 15305
        FishingCatch,
        // Token: 0x04003BCA RID: 15306
        FishingPull,
        // Token: 0x04003BCB RID: 15307
        FishingGiveLine,
        // Token: 0x04003BCC RID: 15308
        FishingFinish,
        // Token: 0x04003BCD RID: 15309
        FishingCancel,
        // Token: 0x04003BCE RID: 15310
        CreateGuildAccessTag,
        // Token: 0x04003BCF RID: 15311
        DeleteGuildAccessTag,
        // Token: 0x04003BD0 RID: 15312
        RenameGuildAccessTag,
        // Token: 0x04003BD1 RID: 15313
        FlagGuildAccessTagGuildPermission,
        // Token: 0x04003BD2 RID: 15314
        AssignGuildAccessTag,
        // Token: 0x04003BD3 RID: 15315
        RemoveGuildAccessTagFromPlayer,
        // Token: 0x04003BD4 RID: 15316
        ModifyGuildAccessTagEditors,
        // Token: 0x04003BD5 RID: 15317
        RequestPublicAccessTags,
        // Token: 0x04003BD6 RID: 15318
        ChangeAccessTagPublicFlag,
        // Token: 0x04003BD7 RID: 15319
        UpdateGuildAccessTag,
        // Token: 0x04003BD8 RID: 15320
        SteamStartMicrotransaction,
        // Token: 0x04003BD9 RID: 15321
        SteamFinishMicrotransaction,
        // Token: 0x04003BDA RID: 15322
        SteamIdHasActiveAccount,
        // Token: 0x04003BDB RID: 15323
        CheckEmailAccountState,
        // Token: 0x04003BDC RID: 15324
        LinkAccountToSteamId,
        // Token: 0x04003BDD RID: 15325
        BuyGvgSeasonBooster,
        // Token: 0x04003BDE RID: 15326
        ChangeFlaggingPrepare,
        // Token: 0x04003BDF RID: 15327
        OverCharge,
        // Token: 0x04003BE0 RID: 15328
        OverChargeEnd,
        // Token: 0x04003BE1 RID: 15329
        RequestTrusted,
        // Token: 0x04003BE2 RID: 15330
        ChangeGuildLogo,
        // Token: 0x04003BE3 RID: 15331
        PartyFinderRegisterForUpdates,
        // Token: 0x04003BE4 RID: 15332
        PartyFinderUnregisterForUpdates,
        // Token: 0x04003BE5 RID: 15333
        PartyFinderEnlistNewPartySearch,
        // Token: 0x04003BE6 RID: 15334
        PartyFinderDeletePartySearch,
        // Token: 0x04003BE7 RID: 15335
        PartyFinderChangePartySearch,
        // Token: 0x04003BE8 RID: 15336
        PartyFinderChangeRole,
        // Token: 0x04003BE9 RID: 15337
        PartyFinderApplyForGroup,
        // Token: 0x04003BEA RID: 15338
        PartyFinderAcceptOrDeclineApplyForGroup,
        // Token: 0x04003BEB RID: 15339
        PartyFinderGetEquipmentSnapshot,
        // Token: 0x04003BEC RID: 15340
        PartyFinderRegisterApplicants,
        // Token: 0x04003BED RID: 15341
        PartyFinderUnregisterApplicants,
        // Token: 0x04003BEE RID: 15342
        PartyFinderFulltextSearch,
        // Token: 0x04003BEF RID: 15343
        PartyFinderRequestEquipmentSnapshot,
        // Token: 0x04003BF0 RID: 15344
        GetPersonalSeasonTrackerData,
        // Token: 0x04003BF1 RID: 15345
        UseConsumableFromInventory,
        // Token: 0x04003BF2 RID: 15346
        ClaimPersonalSeasonReward,
        // Token: 0x04003BF3 RID: 15347
        EasyAntiCheatMessageToServer,
        // Token: 0x04003BF4 RID: 15348
        SetNextTutorialState,
        // Token: 0x04003BF5 RID: 15349
        AddPlayerToMuteList,
        // Token: 0x04003BF6 RID: 15350
        RemovePlayerFromMuteList,
        // Token: 0x04003BF7 RID: 15351
        ProductShopUserEvent,
        // Token: 0x04003BF8 RID: 15352
        GetVanityUnlocks,
        // Token: 0x04003BF9 RID: 15353
        BuyVanityUnlocks,
        // Token: 0x04003BFA RID: 15354
        GetMountSkins,
        // Token: 0x04003BFB RID: 15355
        SetMountSkin,
        // Token: 0x04003BFC RID: 15356
        SetWardrobe,
        // Token: 0x04003BFD RID: 15357
        ChangeCustomization,
        // Token: 0x04003BFE RID: 15358
        SetFavoriteIsland,
        // Token: 0x04003BFF RID: 15359
        GetGuildChallengePoints,
        // Token: 0x04003C00 RID: 15360
        TravelToHideout,
        // Token: 0x04003C01 RID: 15361
        SmartQueueJoin,
        // Token: 0x04003C02 RID: 15362
        SmartQueueLeave,
        // Token: 0x04003C03 RID: 15363
        SmartQueueSelectSpawnCluster,
        // Token: 0x04003C04 RID: 15364
        UpgradeHideout,
        // Token: 0x04003C05 RID: 15365
        InitHideoutAttackStart,
        // Token: 0x04003C06 RID: 15366
        InitHideoutAttackCancel,
        // Token: 0x04003C07 RID: 15367
        HideoutFillNutrition,
        // Token: 0x04003C08 RID: 15368
        HideoutGetInfo,
        // Token: 0x04003C09 RID: 15369
        HideoutGetOwnerInfo,
        // Token: 0x04003C0A RID: 15370
        HideoutSetTribute,
        // Token: 0x04003C0B RID: 15371
        OpenWorldAttackScheduleStart,
        // Token: 0x04003C0C RID: 15372
        OpenWorldAttackScheduleCancel,
        // Token: 0x04003C0D RID: 15373
        OpenWorldAttackConquerStart,
        // Token: 0x04003C0E RID: 15374
        OpenWorldAttackConquerCancel,
        // Token: 0x04003C0F RID: 15375
        GetOpenWorldAttackDetails,
        // Token: 0x04003C10 RID: 15376
        GetNextOpenWorldAttackScheduleTime,
        // Token: 0x04003C11 RID: 15377
        RecoverVaultFromHideout,
        // Token: 0x04003C12 RID: 15378
        GetGuildEnergyDrainInfo
    }
}
