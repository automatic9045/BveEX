double location = BveHacker.Scenario.LocationManager.Location;

if (1550d < location && location < 1650d)
{
	BveHacker.Scenario.Vehicle.Dynamics.RunningResistanceFactorA = 0d;
	BveHacker.Scenario.Vehicle.Dynamics.RunningResistanceFactorB = 50000d;
	BveHacker.Scenario.Vehicle.Dynamics.RunningResistanceFactorC = 0d;
}
else
{
	BveHacker.Scenario.Vehicle.Dynamics.RunningResistanceFactorA = GetVariable<double>("DefaultFactorA");
	BveHacker.Scenario.Vehicle.Dynamics.RunningResistanceFactorB = GetVariable<double>("DefaultFactorB");
	BveHacker.Scenario.Vehicle.Dynamics.RunningResistanceFactorC = GetVariable<double>("DefaultFactorC");
}

return new MapPluginTickResult();