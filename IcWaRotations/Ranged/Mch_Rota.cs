// namespace DefaultRotations.Ranged;
//
// [Rotation("IncognitoWater's Delayed Tools", CombatType.Both, GameVersion = "6.38")]
// [SourceCode(Path = "main/IcWaRotations/Ranged/Mch_Rota.cs")]
// [Api(1)]
// public sealed class MchRotation : MachinistRotation
// {
// 	private const ActionID MarksmansSpitePvPActionId = (ActionID)29415;
// 	private const double LbInPvpThreshold = 30000;
// 	private const float RestTime = 6f;
//
// 	[RotationConfig(CombatType.PvE, Name = "Reassemble with ChainSaw")]
// 	public bool UseMchReassemble { get; set; } = false;
//
// 	[RotationConfig(CombatType.PvP, Name = "Use the LB in PvP when Target is killable by it")]
// 	public bool UseLbInPvp { get; set; } = true;
//
// 	[RotationConfig(CombatType.PvP, Name = "Turn on if you want to FORCE RS to use nothing while in guard in PvP")]
// 	public bool GuardCancel { get; set; } = true;
//
// 	private IBaseAction MarksmansSpitePvP = new BaseAction(MarksmansSpitePvPActionId);
// 	
//
//
// 	protected override IAction? CountDownAction(float remainTime)
// 	{
// 		if (remainTime < CountDownAhead)
// 		{
// 			if (AirAnchorPvE.CanUse(out var act1)) return act1;
// 			if (!AirAnchorPvE.EnoughLevel && HotShotPvE.CanUse(out act1)) return act1;
// 		}
// 		if (remainTime < 2 && UseBurstMedicine(out var act)) return act;
// 		if (remainTime < 5 && ReassemblePvE.CanUse(out act, CanUseOption.UsedUp | CanUseOption.SkipClippingCheck)) return act;
// 		return base.CountDownAction(remainTime);
// 	}
//
//
// 	protected override bool GeneralGCD(out IAction? act)
// 	{
// 		act = null;
// 		
// 		//Overheated
// 		if (AutoCrossbowPvE.CanUse(out act)) return true;
// 		if (HeatBlastPvE.CanUse(out act)) return true;
//
// 		//Long Cds
// 		if (BioblasterPvE.CanUse(out act)) return true;
// 		if (!SpreadShotPvE.CanUse(out _))
// 		{
// 			if (AirAnchorPvE.CanUse(out act)) return true;
// 			if (!AirAnchorPvE.EnoughLevel && HotShotPvE.CanUse(out act)) return true;
//
// 			if (DrillPvE.CanUse(out act)) return true;
// 		}
//
// 		if (!CombatElapsedLessGCD(4) && ChainSawPvE.CanUse(out act)) return true;
//
// 		//Aoe
// 		if (ChainSawPvE.CanUse(out act)) return true;
// 		if (SpreadShotPvE.CanUse(out act)) return true;
//
// 		//Single
// 		if (CleanShotPvE.CanUse(out act)) return true;
// 		if (SlugShotPvE.CanUse(out act)) return true;
// 		if (SplitShotPvE.CanUse(out act)) return true;
// 		
// 		#region PvP
// 		if (GuardCancel && Player.HasStatus(true, StatusID.Guard)) return base.GeneralGCD(out act);
// 		if (LimitBreakLevel==1 && HostileTarget && UseLbInPvp && HostileTarget.CurrentHp < LbInPvpThreshold && MarksmansSpitePvP.CanUse(out act, CanUseOption.SkipClippingCheck)) return true;
//
// 		if (!Player.HasStatus(true, StatusID.Overheated_3149))
// 		{
// 			if (Player.HasStatus(true, StatusID.DrillPrimed))
// 			{
// 				if (DrillPvP.CanUse(out act, CanUseOption.UsedUp)) return true;
// 			}
// 			else if (Player.HasStatus(true, StatusID.BioblasterPrimed) && HostileTarget && HostileTarget.DistanceToPlayer() < BioblasterPvP.Action.Range)
// 			{
// 				if (BioblasterPvP.CanUse(out act, CanUseOption.UsedUp | CanUseOption.SkipAoeCheck)) return true;
// 			}
// 			else if (Player.HasStatus(true, StatusID.AirAnchorPrimed))
// 			{
// 				if (AirAnchorPvP.CanUse(out act, CanUseOption.UsedUp)) return true;
// 			}
// 			else if (Player.HasStatus(true, StatusID.ChainSawPrimed))
// 			{
// 				if (ChainSawPvP.CanUse(out act, CanUseOption.UsedUp | CanUseOption.SkipAoeCheck)) return true;
// 			}
// 		}
//
// 		if (ScattergunPvP.CanUse(out act)) return true;
// 		if (BlastChargePvP.CanUse(out act, CanUseOption.SkipCastingCheck)) return true;
// 		#endregion
//
// 		return base.GeneralGCD(out act);
// 	}
//
// 	protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
// 	{
// 		if (IsBurst)
// 		{
// 			if (UseBurstMedicine(out act)) return true;
// 		}
//
// 		if (UseMchReassemble && ChainSawPvE.EnoughLevel && nextGCD.IsTheSameTo(true, ChainSawPvE))
// 		{
// 			if (ReassemblePvE.CanUse(out act, CanUseOption.UsedUp)) return true;
// 		}
// 		if (RicochetPvE.CanUse(out act)) return true;
// 		if (GaussRoundPvE.CanUse(out act)) return true;
//
// 		if (!DrillPvE.EnoughLevel && nextGCD.IsTheSameTo(true, CleanShotPvE)
// 			|| nextGCD.IsTheSameTo(false, AirAnchorPvE, ChainSawPvE, DrillPvE))
// 		{
// 			if (ReassemblePvE.CanUse(out act, CanUseOption.UsedUp)) return true;
// 		}
// 		
// 		#region PvP
// 		act = null;
//
// 		if (GuardCancel && Player.HasStatus(true, StatusID.Guard)) return base.EmergencyAbility(nextGCD, out act);
//
// 		if (Player.HasStatus(true, StatusID.Overheated_3149) && WildfirePvP.CanUse(out act)) return true;
//
// 		if ((nextGCD.IsTheSameTo(ActionID.DrillPvP) || nextGCD.IsTheSameTo(ActionID.BioblasterPvP) && NumberOfHostilesInRange > 2 || nextGCD.IsTheSameTo(ActionID.AirAnchorPvP)) &&
// 			!(IsLastAction(ActionID.DrillPvP) || IsLastAction(ActionID.BioblasterPvP) || IsLastAction(ActionID.AirAnchorPvP)) && AnalysisPvP.CanUse(out act)) return true;
//
// 		if (BishopAutoturretPvP.CanUse(out act)) return true;
// 		#endregion
// 		
// 		return base.EmergencyAbility(nextGCD, out act);
// 	}
//
// 	protected override bool AttackAbility(IAction nextGCD, out IAction? act)
// 	{
// 		if (IsBurst)
// 		{
// 			if ((IsLastAbility(false, HyperchargePvE) || Heat >= 50) && !CombatElapsedLess(10)
// 				&& WildfirePvE.CanUse(out act, CanUseOption.OnLastAbility)) return true;
// 		}
//
// 		if (!CombatElapsedLess(12) && CanUseHypercharge(out act)) return true;
// 		if (CanUseRookAutoturret(out act)) return true;
//
// 		if (BarrelStabilizerPvE.CanUse(out act)) return true;
//
// 		if (CombatElapsedLess(8)) return false;
//
// 		var option = CanUseOption.UsedUp | CanUseOption.SkipComboCheck;
// 		if (GaussRoundPvE.Cooldown.CurrentCharges <= RicochetPvE.Cooldown.CurrentCharges)
// 		{
// 			if (RicochetPvE.CanUse(out act, option)) return true;
// 		}
// 		if (GaussRoundPvE.CanUse(out act, option)) return true;
//
// 		return base.AttackAbility(nextGCD, out act);
// 	}
//
//
// 	private bool CanUseRookAutoturret(out IAction act)
// 	{
// 		act = null;
// 		if (AirAnchorPvE.EnoughLevel)
// 		{
// 			if (!AirAnchorPvE.IsInCooldown || AirAnchorPvE.Cooldown.ElapsedAfter(18)) return false;
// 		}
// 		else
// 		{
// 			if (!HotShotPvE.IsInCooldown || HotShotPvE.Cooldown.ElapsedAfter(18)) return false;
// 		}
//
// 		return RookAutoturretPvE.CanUse(out act);
// 	}
// 	
// 	private bool CanUseHypercharge(out IAction act)
// 	{
// 		act = null;
//
//
// 		//Check recast.
// 		if (!SpreadShotPvE.CanUse(out _))
// 		{
// 			if (AirAnchorPvE.EnoughLevel)
// 			{
// 				if (AirAnchorPvE.Cooldown.WillHaveOneCharge(RestTime)) return false;
// 			}
// 			else
// 			{
// 				if (HotShotPvE.EnoughLevel && HotShotPvE.Cooldown.WillHaveOneCharge(RestTime)) return false;
// 			}
// 		}
// 		if (DrillPvE.EnoughLevel && DrillPvE.Cooldown.WillHaveOneCharge(RestTime)) return false;
// 		if (ChainSawPvE.EnoughLevel && ChainSawPvE.Cooldown.WillHaveOneCharge(RestTime)) return false;
//
// 		return HyperchargePvE.CanUse(out act);
// 	}
// }