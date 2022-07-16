<?xml version="1.0" encoding="utf-8"?>
<Definitions xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DefinitionList>
    <Definition xsi:type="LivingEntityDefinition" Id="LivingEntity/Hero">
      <BaseAttributes HP_NOW="0" HP_MAX="0" HP_MAX_MLT="0" HP_REGEN="0" EP_NOW="0" EP_MAX="0" EP_MAX_MLT="0" EP_REGEN="0" HIT_CLOSE_RANGE="0" HIT_FAR_RANGE="0" DODGE_CLOSE_RANGE="0" DODGE_FAR_RANGE="0" CRIT_CHANCE="0" CRIT_MLT="0" MOVESPEED="0" MOVESPEED_MLT="0" INITIATIVE="0">
        <STATS STR="0" AGI="0" END="0" INT="0" WIL="0" PER="0" />
        <PERKS CLOSE_WEAPON="0" RANGED_WEAPON="0" SCI_WEAPON="0" HACKING="0" TECH="0" MEDICINE="0" />
        <CRUSH ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <PIERCE ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <CUT ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <FIRE ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <COLD ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <POISON ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <GRAVI ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
      </BaseAttributes>
      <UpgradedAttributes HP_NOW="0" HP_MAX="0" HP_MAX_MLT="0" HP_REGEN="0" EP_NOW="0" EP_MAX="0" EP_MAX_MLT="0" EP_REGEN="0" HIT_CLOSE_RANGE="0" HIT_FAR_RANGE="0" DODGE_CLOSE_RANGE="0" DODGE_FAR_RANGE="0" CRIT_CHANCE="0" CRIT_MLT="0" MOVESPEED="0" MOVESPEED_MLT="0" INITIATIVE="0">
        <STATS STR="0" AGI="0" END="0" INT="0" WIL="0" PER="0" />
        <PERKS CLOSE_WEAPON="0" RANGED_WEAPON="0" SCI_WEAPON="0" HACKING="0" TECH="0" MEDICINE="0" />
        <CRUSH ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <PIERCE ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <CUT ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <FIRE ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <COLD ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <POISON ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <GRAVI ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
      </UpgradedAttributes>
      <Skills>
        <Id>Skill/SimpleAttack</Id>
      </Skills>
      <EquippedItems>
        <Id>Item/WTF</Id>
      </EquippedItems>
    </Definition>
    <Definition xsi:type="SkillDefinition" Id="Skill/SimpleAttack" IsRangedAttack="false" HitChanceMlt="1">
      <Name />
      <Icon />
      <Description />
      <Type />
      <Attacks>
        <Attack Name="CRUSH" Percent="0.5" />
        <Attack Name="FIRE" Percent="0.5" />
      </Attacks>
    </Definition>
    <Definition xsi:type="EquipmentDefinition" Id="Item/BaseWeapon">
      <Attributes HP_NOW="0" HP_MAX="0" HP_MAX_MLT="0" HP_REGEN="0" EP_NOW="0" EP_MAX="0" EP_MAX_MLT="0" EP_REGEN="0" HIT_CLOSE_RANGE="0" HIT_FAR_RANGE="0" DODGE_CLOSE_RANGE="0" DODGE_FAR_RANGE="0" CRIT_CHANCE="0" CRIT_MLT="0" MOVESPEED="0" MOVESPEED_MLT="0" INITIATIVE="0">
        <STATS STR="0" AGI="0" END="0" INT="0" WIL="0" PER="0" />
        <PERKS CLOSE_WEAPON="0" RANGED_WEAPON="0" SCI_WEAPON="0" HACKING="0" TECH="0" MEDICINE="0" />
        <CRUSH ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <PIERCE ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <CUT ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <FIRE ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <COLD ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <POISON ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
        <GRAVI ATK_MIN_ABS="0" ATK_MAX_ABS="0" ATK_MIN_MLT="0" ATK_MAX_MLT="0" DEF_ABS="0" DEF_MLT="0" SPECIAL_1="0" SPECIAL_2="0" />
      </Attributes>
    </Definition>
  </DefinitionList>
</Definitions>