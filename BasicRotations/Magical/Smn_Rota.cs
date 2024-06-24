using System.ComponentModel;

namespace IcWaRotations.Magical;


//RebornUpdatedVersion 1.0
[Rotation("IncognitoWater's SMN", CombatType.Both, GameVersion = "6.38")]
[SourceCode(Path = "main/DefaultRotations/Magical/SMN_Default.cs")]
[Api(1)]
public sealed class SmnRotation : SummonerRotation
{
	public enum SwiftType : byte
	{
		No,
		Emerald,
		Ruby,
		All,
	}
	
	public enum SummonOrderType : byte
	{
		[Description("Topaz-Emerald-Ruby")]
		TopazEmeraldRuby,

		[Description("Topaz-Ruby-Emerald")]
		TopazRubyEmerald,

		[Description("Emerald-Topaz-Ruby")]
		EmeraldTopazRuby,
	}

	[RotationConfig(CombatType.PvE, Name = "Order")]
	public SummonOrderType SummonOrder { get; set; } = SummonOrderType.EmeraldTopazRuby;
	
	[RotationConfig(CombatType.PvE, Name = "SwiftCast")]
	public SwiftType AddSwiftCast { get; set; } = SwiftType.No;
	
	[RotationConfig(CombatType.PvE, Name = "Use Crimson Cyclone. Will use at any range, regardless of saftey use with caution.")]
	public bool AddCrimsonCyclone { get; set; } = true;
	
	[RotationConfig(CombatType.PvE, Name = "Use radiant on cooldown. But still keeping one charge")]
	public bool RadiantOnCooldown { get; set; } = false;
	
	[RotationConfig(CombatType.PvE, Name = "Settings for working with orbwalker")]
	public bool OrbWalkerAdjust { get; set; } = false;
	
	
	[RotationConfig(CombatType.PvP, Name = "Use CrimsonCyclon in PvP")]
	public bool CrimsonCycloneInPvP { get; set; } = false;
	
	[RotationConfig(CombatType.PvP, Name = "Avoid Rs use speel with enemy on Guard in PvP")]
	public bool GuardCancel { get; set; } = false;
	
	[RotationConfig(CombatType.PvP, Name = "Crimson Spcial only on enemy with less than 20k")]
	public bool CrimsonSpecial { get; set; } = false;
	
	
	public override bool CanHealSingleSpell => false;
	
	protected override bool MoveForwardGCD(out IAction? act)
	{
		if (CrimsonCyclonePvE.CanUse(out act, skipAoeCheck: true)) return true;
		return base.MoveForwardGCD(out act);
	}
	

	private float GetPlayerHealthPercent()
	{
		return (Player.CurrentHp / Player.MaxHp) * 100;
	}
	
	protected override bool GeneralGCD(out IAction? act)
	{
		act = null;

		#region PvP
		if (GuardCancel && Player.HasStatus(true, StatusID.Guard)) return false;
		if (Player.HasStatus(true, StatusID.DreadwyrmTrance))
		{
			if (AstralImpulsePvP.CanUse(out act)) return true;
		}
		if (Player.HasStatus(true, StatusID.FirebirdTrance))
		{
			if (FountainOfFirePvP.CanUse(out act)) return true;
		}
		if (CrimsonCycloneInPvP && CrimsonSpecial)
		{
			if ((HostileTarget && HostileTarget.CurrentHp < 20000)
				&&CrimsonCyclonePvP.CanUse(out act)) return true;
		}
		if (CrimsonCycloneInPvP && !CrimsonSpecial)
		{
			if (CrimsonCyclonePvP.CanUse(out act)) return true;
		}
		if (IsLastGCD(ActionID.CrimsonCyclonePvP) && CrimsonStrikePvP.CanUse(out act)) return true;
		if (SlipstreamPvP.CanUse(out act)) return true;
		if (RuinIiiPvP.CanUse(out act)) return true;
		#endregion

		#region PvE
		//Spawning carbuncle + attempting to avoid unwanted try of spawning carbuncle
		if (OrbWalkerAdjust)
		{
			if (!InBahamut && !InPhoenix && !InGaruda && !InIfrit && !InTitan && SummonCarbunclePvE.CanUse(out act)) return true;
		}
		else
		{
			if (!InBahamut && !InPhoenix && !InGaruda && !InIfrit && !InTitan && SummonCarbunclePvE.CanUse(out act)) return true;
		}

		//slipstream
		if (OrbWalkerAdjust)
		{
			if (SlipstreamPvE.CanUse(out act)) return true;
		}
		else
		{
			if (SlipstreamPvE.CanUse(out act)) return true;
		}

		//Crimson strike 
		if (CrimsonStrikePvE.CanUse(out act)) return true;

		//AOE
		if (PreciousBrilliancePvE.CanUse(out act)) return true;
		//gemshine
		if (InIfrit && OrbWalkerAdjust)
		{
			if (GemshinePvE.CanUse(out act)) return true;
		}
		else
		{
			if (GemshinePvE.CanUse(out act)) return true;
		}

		if (AddCrimsonCyclone && CrimsonCyclonePvE.CanUse(out act)) return true;

		//Summon Baha or Phoenix
		if ((Player.HasStatus(false, StatusID.SearingLight) || SearingLightPvE.IsInCooldown) && SummonBahamutPvE.CanUse(out act)) return true;
		if (!SummonBahamutPvE.EnoughLevel && HasHostilesInRange && AetherchargePvE.CanUse(out act)) return true;

		//Ruin4
		if (IsMoving && InIfrit
			&& !Player.HasStatus(true, StatusID.Swiftcast) && !InBahamut && !InPhoenix
			&& RuinIvPvE.CanUse(out act)) return true;

		//Select summon order
		switch (SummonOrder)
		{
		case SummonOrderType.TopazEmeraldRuby:
			//Titan
			if (SummonTopazPvE.CanUse(out act)) return true;
			//Garuda
			if (SummonEmeraldPvE.CanUse(out act)) return true;
			//Ifrit
			if (SummonRubyPvE.CanUse(out act)) return true;
			break;

		case SummonOrderType.TopazRubyEmerald:
			//Titan
			if (SummonTopazPvE.CanUse(out act)) return true;
			//Ifrit
			if (SummonRubyPvE.CanUse(out act)) return true;
			//Garuda
			if (SummonEmeraldPvE.CanUse(out act)) return true;
			break;

		case SummonOrderType.EmeraldTopazRuby:
			//Garuda
			if (SummonEmeraldPvE.CanUse(out act)) return true;
			//Titan
			if (SummonTopazPvE.CanUse(out act)) return true;
			//Ifrit
			if (SummonRubyPvE.CanUse(out act)) return true;
			break;
		}
		if (SummonTimeEndAfterGCD() && AttunmentTimeEndAfterGCD() && SummonBahamutPvE.Cooldown.ElapsedAfter(20) &&
			!Player.HasStatus(true, StatusID.Swiftcast) && !InBahamut && !InPhoenix && !InGaruda && !InTitan &&
			RuinIvPvE.CanUse(out act)) return true;
		//Outburst
		if (OrbWalkerAdjust)
		{
			if (OutburstPvE.CanUse(out act)) return true;
		}
		else
		{
			if (OutburstPvE.CanUse(out act)) return true;
		}

		//Any ruin ( 1-2-3 ) 
		if (OrbWalkerAdjust)
		{
			if (RuinIvPvE.CanUse(out act)) return true;
		}
		else
		{
			if (RuinPvE.CanUse(out act)) return true;
		}
		return base.GeneralGCD(out act);
		#endregion
	}

	protected override bool AttackAbility(IAction nextGCD,out IAction? act)
	{
		#region PvE
		if (IsBurst && !Player.HasStatus(false, StatusID.SearingLight))
		{
			//Burst raidbuff searinglight
			if (SearingLightPvE.CanUse(out act, skipAoeCheck:true)) return true;
		}


		//Burst for bahamut
		if ((InBahamut && SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(3) || InPhoenix || (HostileTarget.IsBossFromIcon() && HostileTarget.IsDying())) && EnkindleBahamutPvE.CanUse(out act)) return true;
		//Burst second part for bahamut
		if ((InBahamut && SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(3) || (HostileTarget.IsBossFromIcon() && HostileTarget.IsDying())) && DeathflarePvE.CanUse(out act)) return true;
		//Change rekindle timing to avoid triple weaving issue if animation are unlocked
		if (InPhoenix && SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(1) && RekindlePvE.CanUse(out act)) return true;
		//Special Titan
		if (MountainBusterPvE.CanUse(out act)) return true;

		//Painflare timing for tincture and rotation
		if ((Player.HasStatus(false, StatusID.SearingLight) && InBahamut && (SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(4) || ((!EnergyDrainPvE.Cooldown.IsCoolingDown || EnergyDrainPvE.Cooldown.ElapsedAfter(50))) && SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(1)) ||
			!SearingLightPvE.EnoughLevel || (HostileTarget.IsBossFromIcon() && HostileTarget.IsDying())) && PainflarePvE.CanUse(out act)) return true;
		//fester timing for tincture and rotation
		if ((Player.HasStatus(false, StatusID.SearingLight) && InBahamut && (SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(4) || ((!EnergyDrainPvE.Cooldown.IsCoolingDown || EnergyDrainPvE.Cooldown.ElapsedAfter(50))) && SummonBahamutPvE.Cooldown.ElapsedOneChargeAfterGCD(1)) ||
			!SearingLightPvE.EnoughLevel || (HostileTarget.IsBossFromIcon() && HostileTarget.IsDying())) && FesterPvE.CanUse(out act)) return true;

		//energy siphon recharge
		if (EnergySiphonPvE.CanUse(out act)) return true;
		//energy drain recharge
		if (EnergyDrainPvE.CanUse(out act)) return true;
		
		return base.AttackAbility(nextGCD, out act);
		#endregion
	}

	protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
	{
		act = null;

		#region PvP
		if (GuardCancel && Player.HasStatus(true, StatusID.Guard)) return false;

		if (GetPlayerHealthPercent() < 50 && RadiantAegisPvP.CanUse(out act)) return true;
		if (Player.HasStatus(true, StatusID.DreadwyrmTrance))
		{
			if (EnkindleBahamutPvP.CanUse(out act)) return true;
		}
		if (Player.HasStatus(true, StatusID.FirebirdTrance))
		{
			if (EnkindlePhoenixPvP.CanUse(out act)) return true;
		}
		if (MountainBusterPvP.CanUse(out act)) return true;
		if (FesterPvP.CanUse(out act, CanUseOption.UsedUp)) return true;
		#endregion

		#region PvE
		// Adding tincture timing to rotations
		if (((SearingLightPvE.Cooldown.IsCoolingDown || Player.HasStatus(false, StatusID.SearingLight)) && InBahamut) && (UseBurstMedicine(out act))) return true;

		// moved swift usage on emergency to avoid unsended swift
		switch (AddSwiftCast)
		{
		case SwiftType.Emerald:
			if (InGaruda && Player.Level > 86)
			{
				if (SwiftcastPvE.CanUse(out act)) return true;
			}
			break;
		case SwiftType.Ruby:
			if (InIfrit)
			{
				if (SwiftcastPvE.CanUse(out act)) return true;
			}
			break;
		case SwiftType.All:
			if ((InGaruda && Player.Level > 86) || InIfrit)
			{
				if (SwiftcastPvE.CanUse(out act)) return true;
			}
			break;
		case SwiftType.No:
			break;
		default:
			break;
		}

		if (RadiantOnCooldown && (RadiantAegisPvE.Cooldown.CurrentCharges == 2) && SummonBahamutPvE.Cooldown.IsCoolingDown && RadiantAegisPvE.CanUse(out act)) return true;
		if (RadiantOnCooldown && Player.Level < 88 && SummonBahamutPvE.IsInCooldown && RadiantAegisPvE.CanUse(out act, CanUseOption.UsedUp)) return true;

		return base.EmergencyAbility(nextGCD, out act);
		#endregion
	}

	protected override IAction CountDownAction(float remainTime)
	{
		if (SummonCarbunclePvE.CanUse(out _)) return SummonCarbunclePvE;
		//1.5s prepull ruin 
		if (remainTime <= RuinPvE.Info.CastTime + CountDownAhead
			&& RuinPvE.CanUse(out _)) return RuinPvE;
		return base.CountDownAction(remainTime);
	}
}