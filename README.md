# BtoFanControl

This application regulates the fan speed of Clevo / BTO laptops. It was written because the provided "Control Centre 2.0" made my laptop very noisy when running in "Performance" mode, often running the fan needlessly at 100%. 

## Features
- Monitors CPU temperature and adjusts fan speed accordingly.
- Uses only a few different fan speeds (22%, 30%, 50% and 70%).
- Switches back to lower fan speed only when the average temperature drops significantly below the temperature that triggered the current fan speed (hysteresis).
- This results in much quieter operation, and prevents the constant ramping up and down of fan speeds (it's the changing noise that you notice, more so than the actual level of noise).

## Usage
- Just extract the contents of the zip file into a folder, and run the exe as an administrator. A console window will display something like this:
```console
Average = 43°C, Current = 43°C, Fan% should be 22% => Fan% is now 22%
Last updated fan speed at 18:05:14. 6 updates so far.
```

## Limitations
- BtoFanControl needs to be "run as administrator". OpenHardwareMonitor requires it to be able to read the CPU temperatures.
- There is no posibility for configuration whatsoever (unless you clone the repo and compile your own version).
- It controls only one fan.
- This project will not be maintained. Feel free to use it as is, or fork it an make your own version.
- It uses "ClevoEcInfo.dll" to set the fan speed. This dll seems to have some stability issues. Before switching from "ClevoEcInfo.dll" to OpenHardwareMonitor for measuring the CPU temperature this resulted in the occasional "driver-power-state-failure". It hasn't happened since, but it still might.
