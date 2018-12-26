# combo
Minimize the IT technology - the head count of Alitum hardware engineer who design from scratch is below 10 in the organization. No need for infrastructure so far. Storage the part information locally, no server needed.

Why Access Database
1. Suppported by Altium
2. Light weight Generally installed on Windows PC in the organization
3. Easily ported to any other fancy DB when the business needs scaling up.

Essential Parts:
1. DbLib - Data inferface, merge Accdb - text, SchLib - symbol, PcbLib - footprint together.
2. Accdb - the text infomation of the components
3. SchLib - Schematic symbols
4. PcbLib - PCB footprints.

What does "Combo" do:
1. Editing Accdb and DbLib for batch component creatation and management
2. Octopart API -- massive import information
3. SchLib and PcbLib are edited by Altium.
4. Accdb can be edited by MS Access as well.
