<?xml version="1.0" encoding="utf-8"?>
<Definitions xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DefinitionList>
    <Definition xsi:type="AIBehaviorDefinition">
      <Positioning>
        <Layers>
          <Layer MinValue="1" MaxValue="3" ValueFx="StayAwayFromEnemies">
            <Params>
              <Param Name="MinRange" Value="3" />
              <Param Name="MaxRange" Value="8" />
            </Params>
          </Layer>
        </Layers>
      </Positioning>
      <Behavior>
        <Action xsi:type="AIBehaviorUseSkillXml" SkillType="Attack" TargetFilter="Ally" TargetSelectorFx="Closest" MaxMoveAPToCast="2" />
      </Behavior>
      <Triggers>
        <Trigger xsi:type="AIBehaviorTriggerUnitDiedXml" TriggerFx="Say" Ally="true">
          <Params>
            <Param Name="Text" Value="You bastard!" />
          </Params>
        </Trigger>
      </Triggers>
    </Definition>
  </DefinitionList>
</Definitions>