// namespace IcWaRotations.Magical;
//
// [Rotation("IncognitoWater's RDM", CombatType.PvE, GameVersion = "6.38")]
// [SourceCode(Path = "main/IcWaRotations/Magical/Rdm_Rota.cs")]
// [Api(1)]
// public sealed class RdmRotation : RedMageRotation
// {
//
// 	[RotationConfig(CombatType.PvE, Name = "Use Vercure for Dualcast when out of combat.")]
// 	public bool UseVercure { get; set; } = false;
// 	
// 	[RotationConfig(CombatType.PvE, Name = "Use Acceleration On Cooldown to avoid overstack")]
// 	public bool UseAcceleration { get; set; } = false;
// 	
// 	private bool CanStartMeleeCombo
// 	{
// 		get
// 		{
// 			if (Player.HasStatus(true, StatusID.Manafication, StatusID.Embolden) ||
// 				BlackMana == 100 || WhiteMana == 100) return true;
//
// 			if (BlackMana == WhiteMana) return false;
//
// 			if (WhiteMana < BlackMana)
// 			{
// 				if (Player.HasStatus(true, StatusID.VerstoneReady)) return false;
// 			}
// 			else
// 			{
// 				if (Player.HasStatus(true, StatusID.VerfireReady)) return false;
// 			}
//
// 			if (Player.HasStatus(true, VercurePvE.Setting.StatusProvide)) return false;
//
// 			//Waiting for embolden.
// 			if (EmboldenPvE.EnoughLevel && EmboldenPvE.Cooldown.WillHaveOneChargeGCD(5)) return false;
//
// 			return true;
// 		}
// 	}
// 	
//
// 	protected override IAction CountDownAction(float remainTime)
// 	{
// 		if (remainTime < VerthunderIiiPvE.Info.CastTime + CountDownAhead
// 			&& VerthunderIiiPvE.CanUse(out var act, CanUseOption.SkipComboCheck)) return act;
//
// 		//Remove Swift
// 		StatusHelper.StatusOff(StatusID.Dualcast);
// 		StatusHelper.StatusOff(StatusID.Acceleration);
// 		StatusHelper.StatusOff(StatusID.Swiftcast);
//
// 		return base.CountDownAction(remainTime);
// 	}
//
// 	protected override bool GeneralGCD(out IAction act)
// 	{
// 		act = null;
// 		if (ManaStacks == 3) return false;
//
// 		if (!VerthunderIiiPvE.CanUse(out _))
// 		{
// 			if (VerfirePvE.CanUse(out act)) return true;
// 			if (VerstonePvE.CanUse(out act)) return true;
// 		}
//
// 		if (ScatterPvE.CanUse(out act)) return true;
// 		if (WhiteMana < BlackMana)
// 		{
// 			if (VeraeroIiiPvE.CanUse(out act) && BlackMana - WhiteMana != 5) return true;
// 			if (VeraeroPvE.CanUse(out act) && BlackMana - WhiteMana != 6) return true;
// 		}
//
// 		if (VerthunderIiiPvE.CanUse(out act)) return true;
// 		if (VerthunderPvE.CanUse(out act)) return true;
//
// 		if (JoltPvE.CanUse(out act)) return true;
//
// 		if (UseVercure && NotInCombatDelay && VercurePvE.CanUse(out act)) return true;
//
// 		return base.GeneralGCD(out act);
// 	}
//
// 	protected override bool EmergencyGCD(out IAction act)
// 	{
// 		if (ManaStacks == 3)
// 		{
// 			if (BlackMana > WhiteMana)
// 			{
// 				if (VerholyPvE.CanUse(out act)) return true;
// 			}
//
// 			if (VerflarePvE.CanUse(out act)) return true;
// 		}
//
// 		if (ResolutionPvE.CanUse(out act)) return true;
// 		if (ScorchPvE.CanUse(out act)) return true;
//
//
// 		if (IsLastGCD(true, MoulinetPvE) && MoulinetPvE.CanUse(out act)) return true;
// 		if (ZwerchhauPvE.CanUse(out act)) return true;
// 		if (RedoublementPvE.CanUse(out act)) return true;
//
// 		if (!CanStartMeleeCombo) return false;
//
// 		if (MoulinetPvE.CanUse(out act))
// 		{
// 			if (BlackMana >= 60 && WhiteMana >= 60) return true;
// 		}
// 		else
// 		{
// 			if (BlackMana >= 50 && WhiteMana >= 50 && RipostePvE.CanUse(out act)) return true;
// 		}
//
// 		if (ManaStacks > 0 && RipostePvE.CanUse(out act)) return true;
//
// 		return base.EmergencyGCD(out act);
// 	}
//
// 	protected override bool EmergencyAbility(IAction nextGCD, out IAction act)
// 	{
// 		act = null;
//
// 		if (IsBurst && UseBurstMedicine(out act)) return true;
//
// 		if (CombatElapsedLess(4)) return false;
// 		if (ScorchPvE.CanUse(out act)) return false;
// 		if (ResolutionPvE.CanUse(out act)) return false;
//
// 		if (UseAcceleration && AccelerationPvE.Cooldown.CurrentCharges == 2 &&
// 			AccelerationPvE.CanUse(out act)) return true;
//
// 		if (IsBurst && HasHostilesInRange && EmboldenPvE.CanUse(out act)) return true;
//
// 		if (((Player.HasStatus(true, StatusID.Embolden) || IsLastAbility(ActionID.EmboldenPvE)) && ManaStacks == 0 && WhiteMana <= 50 && BlackMana <= 50) &&
// 			ManaficationPvE.CanUse(out act)) return true;
//
// 		return base.EmergencyAbility(nextGCD, out act);
// 	}
//
// 	protected override bool AttackAbility(IAction nextGCD,out IAction act)
// 	{
// 		//Swift
// 		if (ManaStacks == 0 && (BlackMana < 50 || WhiteMana < 50)
// 			&& (CombatElapsedLess(4) || !ManaficationPvE.EnoughLevel ||
// 				!ManaficationPvE.Cooldown.WillHaveOneChargeGCD(0, 1)))
// 		{
// 			if (!Player.HasStatus(true, StatusID.VerfireReady, StatusID.VerstoneReady))
// 			{
// 				if (SwiftcastPvE.CanUse(out act)) return true;
// 				if (InCombat && AccelerationPvE.CanUse(out act, CanUseOption.SkipComboCheck)) return true;
// 			}
// 		}
//
// 		//Attack abilities.
// 		if (ContreSixtePvE.CanUse(out act)) return true;
// 		if (FlechePvE.CanUse(out act)) return true;
//
// 		if (EngagementPvE.CanUse(out act, CanUseOption.SkipComboCheck)) return true;
// 		if (CorpsacorpsPvE.CanUse(out act) && !IsMoving) return true;
//
// 		return base.AttackAbility(nextGCD, out act);
// 	}
// }