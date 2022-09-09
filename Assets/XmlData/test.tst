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
              <FxParamXml>
                <Name>MinRange</Name>
                <Value>3</Value>
              </FxParamXml>
              <FxParamXml>
                <Name>MaxRange</Name>
                <Value>8</Value>
              </FxParamXml>
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
            <Param>
              <Name>Text</Name>
              <Value>You bastard!</Value>
            </Param>
          </Params>
        </AIBehaviorTriggerXml>
      </Triggers>
    </Definition>
  </DefinitionList>
</Definitions>