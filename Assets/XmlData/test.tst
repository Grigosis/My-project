<?xml version="1.0" encoding="utf-8"?>
<Definitions xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DefinitionList>
    <Definition xsi:type="AIBehaviorDefinition">
      <name />
      <hideFlags>None</hideFlags>
      <Positioning>
        <Layers>
          <AIPositioningLayerXml>
            <ValueFx>StayAwayFromEnemies</ValueFx>
            <MinValue>1</MinValue>
            <MaxValue>3</MaxValue>
            <Params>
              <FxParamXml Name="MinRange" Value="3" />
              <FxParamXml Name="MaxRange" Value="8" />
            </Params>
          </AIPositioningLayerXml>
        </Layers>
      </Positioning>
      <Behavior>
        <AIBehaviorActionXml xsi:type="AIBehaviorUseSkillXml" SkillType="Attack" TargetFilter="Ally" TargetSelectorFx="Closest" MaxMoveAPToCast="2" />
      </Behavior>
      <Triggers>
        <AIBehaviorTriggerXml xsi:type="AIBehaviorTriggerUnitDiedXml" TriggerFx="Say" Ally="true">
          <Params>
            <Param Name="Text" Value="You bastard!" />
          </Params>
        </AIBehaviorTriggerXml>
      </Triggers>
    </Definition>
  </DefinitionList>
</Definitions>