#####################################################################################
#
# Copyright (c) SiRF Technology Inc. 
#
#####################################################################################
clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import *

from System import Array
import time

import scriptGlobals

def str2IntArray(inputArray):
	intArray = array.array ('i')
	for elm in inputArray:
		intArray.append(int(elm))
	return intArray

def scriptGlobalsInit():
	scriptGlobals.MainFrame= object()
	scriptGlobals.Console= object()
	
	scriptGlobals.TestAborted= False
	scriptGlobals.Exiting= False
	
	# RF playback params
	scriptGlobals.DeviceID=0
	scriptGlobals.RFPlaybackConfigParams=[]
	scriptGlobals.PlayTimeList = []
	scriptGlobals.FileToPlayList = []
	scriptGlobals.SiteNamesList=[]
	
	# Sim params
	scriptGlobals.SimAddr = "127.0.0.1"
	scriptGlobals.SimPort = 15650
	scriptGlobals.SimFile = ""
	scriptGlobals.SimInitialAtten=0
	
	# Test setups
	scriptGlobals.BaseLogDirectory=""
	scriptGlobals.SignalSource=""
	scriptGlobals.PowerSource=""
	scriptGlobals.AttenSource=""
	scriptGlobals.IsAidingTest=0
	scriptGlobals.TestResultsDirectory=""
	scriptGlobals.LogFilesList=[]
	scriptGlobals.TTFFLogFilesList=[]
	scriptGlobals.LogFileExtsList=[]
	scriptGlobals.SignalType=""
	scriptGlobals.CableLoss=0.0
	scriptGlobals.TestSignalLevelsList=[]
	scriptGlobals.ScriptConfigFilePath=""
	scriptGlobals.UseTTBForAiding=0
	scriptGlobals.ScriptName=""
	
	# host app
	scriptGlobals.HostAppDirectory=[]
	scriptGlobals.HostAppFilesList=[]
	scriptGlobals.HostAppArgumentsList=[]
	scriptGlobals.HostAppExtraArgumentsList=[]
	scriptGlobals.HostAppRunRequiredList=[]
	scriptGlobals.PatchRunRequiredList=[]
	scriptGlobals.PatchFilesList=[]
	
	# DUT setups
	scriptGlobals.DutMainPortsList=[]
	scriptGlobals.DutTTBPortsList=[]
	scriptGlobals.DutTrackerPortsList=[]
	scriptGlobals.DutHostPortsList=[]
	scriptGlobals.DutResetPorstList=[]
	scriptGlobals.DutBaudRateList=[]
	scriptGlobals.DutNamesList=[]
	scriptGlobals.DutTypesList=[]
	scriptGlobals.DutRxLogTypesList=[]
	scriptGlobals.DutTxLogTypesList=[]
	scriptGlobals.DutDefaultClksList=[]
	scriptGlobals.DutMessageProtocolsList=[]
	scriptGlobals.DutAidingProtocolsList=[]
	scriptGlobals.DutAi3ICDsList=[]
	scriptGlobals.DutFICDsList=[]
	scriptGlobals.DutExtClkFreqsList=[]
	scriptGlobals.DutDefaultClkIndicesList=[]
	scriptGlobals.DutSourceDeviceList=[]
	scriptGlobals.DutFamilyList=[]
	scriptGlobals.DutLNATypesList=[]
	scriptGlobals.DutLDOModesList=[]
	scriptGlobals.DutIPAddrsList=[]
	scriptGlobals.DutSiRFNavInterfaceStringsList=[]
	scriptGlobals.DUTPlatform=[]
	scriptGlobals.DUTRev=[]
	scriptGlobals.DUTMfg=[]
	scriptGlobals.DUTPackage=[]
	scriptGlobals.DUTRefFreqSrc=[]
	scriptGlobals.DUTTemperature=[]
	scriptGlobals.DUTTemperatureUnit=[]
	
	scriptGlobals.DutPortManagersList = []
	
	# Test params
	getNavs = []
	getTTFFs = []
	RefX = 0
	RefY = 0
	RefZ = 0
	RefLat = 0
	RefLon = 0
	RefAlt = 0

try: 
	print "Initializing global variables..."
	scriptGlobalsInit()
	scriptGlobals.MainFrame = mainFrame
	scriptGlobals.Console = mainFrame.GetPythonWindowRef()
	scriptGlobals.MainFrame.SetScriptDone(False)
	General.clsGlobal.ScriptError = False
	currentDir = nt.getcwd()
	# Read configuration parameters
	# Data in pair key and value
	cfg = ConfigParser.ConfigParser()
	#cfgFilePath = "..\\scripts\\SiRFLiveAutomationSetup.cfg"
	cfgFilePath = mainFrame.SetupScriptsPath + "\\scripts\\SiRFLiveAutomationSetup.cfg" 
	cfg.read(cfgFilePath)
	
	scriptGlobals.TestOperator = cfg.get('TEST_SETUP','TEST_OPERATOR')
	
	#deviceId = cfg.getint('RF_PLAYBACK','RF_PLAYER_DEVICE_ID')
	scriptGlobals.DeviceID = cfg.getint('RF_PLAYBACK','RF_PLAYER_DEVICE_ID')
	
	tmpArray = cfg.get('RF_PLAYBACK','SETUP_PARAMS').split(",")
	configParams = str2IntArray(tmpArray)
	#convert to type Array
	#configParams = Array[int](configParams)
	scriptGlobals.RFPlaybackConfigParams = Array[int](configParams)
	
	playTimeList = []
	tmpArray = cfg.get('RF_PLAYBACK','PLAY_TIME_LISTS').split(",")
	for tmp1 in tmpArray:
		tmp2 = tmp1.split(":")
		# playTime expect type Array 
		playTimeList.append(Array[int](str2IntArray(tmp2))) 
	
	scriptGlobals.PlayTimeList = playTimeList 
	
	scriptGlobals.FileToPlayList = cfg.get('RF_PLAYBACK','PLAYBACK_FILES').split(",")
	fileToPlayList = scriptGlobals.FileToPlayList
	
	# get SIM
	mainFrame.SimCtrl.IPAddress =  cfg.get('SIM','SIM_ADDRESS')
	scriptGlobals.SimAddr = mainFrame.SimCtrl.IPAddress
	# mainFrame.SimCtrl.Port = "15650" 	
	mainFrame.SimCtrl.Port =  cfg.get('SIM','SIM_PORT')
	scriptGlobals.SimPort = mainFrame.SimCtrl.Port
	
	# SIM FILE
	# mainSimFile = cfg.get('SIM','SIM_FILE')	
	scriptGlobals.SimFile = cfg.get('SIM','SIM_FILE')
	#mainSimInitAtten = cfg.getfloat('SIM', 'SIM_START_ATTEN');
	scriptGlobals.SimInitialAtten = cfg.getfloat('SIM', 'SIM_START_ATTEN')
	
	NUMBER_FILES_TO_PLAY = cfg.getint('TEST_SETUP','NUMBER_FILES_TO_PLAY')
	NUMBER_FILES_TO_PLAY = NUMBER_FILES_TO_PLAY - 1
	if (NUMBER_FILES_TO_PLAY < 0):
		if (len(fileToPlayList) > 0):
			NUMBER_FILES_TO_PLAY = len(fileToPlayList) - 1
		else:
			NUMBER_FILES_TO_PLAY = len(fileToPlayList) 
	if (NUMBER_FILES_TO_PLAY > len(fileToPlayList)):
		NUMBER_FILES_TO_PLAY = len(fileToPlayList)
	
	baseLogDir = cfg.get('TEST_SETUP','BASE_TEST_LOG')
	baseLogDir = baseLogDir.lstrip()
	baseLogDir = baseLogDir.rstrip()
	scriptGlobals.BaseLogDirectory = baseLogDir
	# get signal source
	# mainSignalSource = cfg.get('TEST_SETUP','SIGNAL_SOURCE')
	scriptGlobals.SignalSource = cfg.get('TEST_SETUP','SIGNAL_SOURCE')
	# get attenuator source
	# mainAttenSource = cfg.get('TEST_SETUP','ATTEN_SOURCE')
	scriptGlobals.AttenSource = cfg.get('TEST_SETUP','ATTEN_SOURCE')
	# get power source
	# mainPowerSource = cfg.get('TEST_SETUP','POWER_SOURCE')
	scriptGlobals.PowerSource = cfg.get('TEST_SETUP','POWER_SOURCE')
	
	# isAidingTest = cfg.getint('TEST_SETUP','REQUIRED_AIDING')
	scriptGlobals.IsAidingTest = cfg.getint('TEST_SETUP','REQUIRED_AIDING')
	
	tmpArray = cfg.get('RX_SETUP','ACTIVE_PORTS').split(",")
	availablePortsList = str2IntArray(tmpArray)
	
	tmpArray = cfg.get('RX_SETUP','TTB_PORTS').split(",")
	availableTTBPortsList = str2IntArray(tmpArray)
	
	NUM_COM_IN_USE = cfg.getint('TEST_SETUP','NUM_RX_IN_TEST')
	if (NUM_COM_IN_USE < 0):
		NUM_COM_IN_USE = len(availablePortsList)
	if (NUM_COM_IN_USE >len(availablePortsList)):
		NUM_COM_IN_USE = len(availablePortsList)
	
	# portList the number of RX in test 
	# the main script will loop through portList to open com 	
	portList = []
	TTBPortList = []
	
	for testComIdx in range(NUM_COM_IN_USE):
		if (availablePortsList[testComIdx] == -1):
			print "Port = %d is invalid" % (availablePortsList[testComIdx])
			#continue	
		portList.append(availablePortsList[testComIdx])
		TTBPortList.append(availableTTBPortsList[testComIdx])	
	
	scriptGlobals.DutMainPortsList = portList
	scriptGlobals.DutTTBPortsList = TTBPortList
	
	tmpArray = cfg.get('RX_SETUP','TRACKER_PORTS').split(",")
	scriptGlobals.DutTrackerPortsList = str2IntArray(tmpArray)
	trackerPortList = scriptGlobals.DutTrackerPortsList
	
	DutPortMangerList = []
	tmpArray = cfg.get('RX_SETUP','HOST_PORTS').split(",")	
	
	scriptGlobals.DutHostPortsList = str2IntArray(tmpArray)
	hostPortList = scriptGlobals.DutHostPortsList 
	
	tmpArray = cfg.get('RX_SETUP','RESET_PORTS').split(",")	
	scriptGlobals.DutResetPortsList= str2IntArray(tmpArray)
	resetPortList = scriptGlobals.DutResetPortsList
	
	# add quotes around
	scriptGlobals.HostAppDirectory =  cfg.get('RX_SETUP','HOST_APP_DIR')
	mainHostAppDir = scriptGlobals.HostAppDirectory
	
	scriptGlobals.HostAppFilesList = cfg.get('RX_SETUP','HOST_APP').split(",")
	mainHostAppFile = scriptGlobals.HostAppFilesList
	
	hostAppDirs = []
	hostAppArgvs = []
	
	scriptGlobals.HostAppExtraArgumentsList= cfg.get('RX_SETUP','EXTRA_HOST_APP_ARGVS').split(",")
	hostAppExtraArgvs = scriptGlobals.HostAppExtraArgumentsList
	
	tmpArray = cfg.get('RX_SETUP','REQUIRED_HOSTS').split(",")
	scriptGlobals.HostAppRunRequiredList = str2IntArray(tmpArray)
	hostAppRequired = scriptGlobals.HostAppRunRequiredList
	
	tmpArray = cfg.get('RX_SETUP','REQUIRED_PATCH').split(",")
	scriptGlobals.PatchRunRequiredList = str2IntArray(tmpArray)
	patchRequired = []
	patchRequired = scriptGlobals.PatchRunRequiredList
	
	scriptGlobals.PatchFilesList = cfg.get('RX_SETUP','PATCH_FILE').split(",")
	patchFiles = []
	patchFiles = scriptGlobals.PatchFilesList
	
	# rxBaudList = cfg.get('RX_SETUP','RX_BAUDS').split(",")	
	scriptGlobals.DutBaudRateList = cfg.get('RX_SETUP','RX_BAUDS').split(",")
	
	# rxNameList = cfg.get('RX_SETUP','RX_NAMES').split(",")
	scriptGlobals.DutNamesList =  cfg.get('RX_SETUP','RX_NAMES').split(",")
	
	# rxTypeList = cfg.get('RX_SETUP','RX_TYPES').split(",")
	scriptGlobals.DutTypesList = cfg.get('RX_SETUP','RX_TYPES').split(",")
	
	# rxLogTypeList = cfg.get('RX_SETUP','RX_LOG_TYPES').split(",")
	scriptGlobals.DutRxLogTypesList = cfg.get('RX_SETUP','RX_LOG_TYPES').split(",")
	
	# txLogTypeList = cfg.get('RX_SETUP','TX_LOG_TYPES').split(",")
	scriptGlobals.DutTxLogTypesList = cfg.get('RX_SETUP','TX_LOG_TYPES').split(",")
	
	# rxMessageProtocol = cfg.get('RX_SETUP','MESSAGE_PROTOCOL').split(",")
	scriptGlobals.DutMessageProtocolsList = cfg.get('RX_SETUP','MESSAGE_PROTOCOL').split(",")
	
	# rxAidingProtocol = cfg.get('RX_SETUP','AIDING_PROTOCOL').split(",")
	scriptGlobals.DutAidingProtocolsList = cfg.get('RX_SETUP','AIDING_PROTOCOL').split(",")
	
	# rxAi3ICDList = cfg.get('RX_SETUP','AI3_ICD').split(",")
	scriptGlobals.DutAi3ICDsList = cfg.get('RX_SETUP','AI3_ICD').split(",")
	
	# rxFICDList = cfg.get('RX_SETUP','F_ICD').split(",")
	scriptGlobals.DutFICDsList =  cfg.get('RX_SETUP','F_ICD').split(",")
	
	# rxExtClkFreq = cfg.get('RX_SETUP', 'EXT_CLK_FREQ').split(",")
	scriptGlobals.DutExtClkFreqsList = cfg.get('RX_SETUP', 'EXT_CLK_FREQ').split(",")
	
	scriptGlobals.DutDefaultClksList = cfg.get('RX_SETUP','DEFAULT_CLK').split(",")
	rxDefaultClk = scriptGlobals.DutDefaultClksList
	rxDefaultClkIdx = []
	for freqIdx in range(len(rxDefaultClk)):
		foundFreqIdx = False
		# print len(General.clsGlobal.DEFAULT_RF_FREQ)
		for freqIdx1 in range(len(General.clsGlobal.DEFAULT_RF_FREQ)):
			if (rxDefaultClk[freqIdx] == `General.clsGlobal.DEFAULT_RF_FREQ[freqIdx1]`):
				rxDefaultClkIdx.Add(freqIdx1)
				foundFreqIdx = True
				break;
		if (foundFreqIdx == False):
			rxDefaultClkIdx.Add(2);
	
	scriptGlobals.DutDefaultClkIndicesList = rxDefaultClkIdx
	
	# rxSourceDevice = []
	# rxSourceDevice = cfg.get('RX_SETUP','PHY_COMM_PROTOCOL').split(",")
	scriptGlobals.DutSourceDeviceList = cfg.get('RX_SETUP','PHY_COMM_PROTOCOL').split(",")
	
	# rxFamily = cfg.get('RX_SETUP','PROD_FAMILY').split(",")
	scriptGlobals.DutFamilyList = cfg.get('RX_SETUP','PROD_FAMILY').split(",")
	
	# rxLNAType = []
	scriptGlobals.DutLNATypesStringList = cfg.get('RX_SETUP','LNA_TYPE').split(",")
	# rxLNAType = str2IntArray(tmpArray)
	scriptGlobals.DutLNATypesList = str2IntArray(scriptGlobals.DutLNATypesStringList)
	
	# rxLDOMode = []
	scriptGlobals.DutLDOModesStringList = cfg.get('RX_SETUP','LDO_MODE').split(",")
	# rxLDOMode = str2IntArray(tmpArray)
	scriptGlobals.DutLDOModesList = str2IntArray(scriptGlobals.DutLDOModesStringList)
	
	# rxIPAddress = cfg.get('RX_SETUP','TCPIP_IP_ADDRESS').split(",")	
	scriptGlobals.DutIPAddrsList = cfg.get('RX_SETUP','TCPIP_IP_ADDRESS').split(",")
	
	# sirfNavIntfStr = cfg.get('RX_SETUP','SIRFNAV_INTF_STR').split(",")
	scriptGlobals.DutSiRFNavInterfaceStringsList = cfg.get('RX_SETUP','SIRFNAV_INTF_STR').split(",")
	
	# winComList to store comm windows reference
	winComList = []
	testPortList = []
	
	scriptGlobals.DUTPlatform = cfg.get('RX_SETUP','PLATFORM').split(",")
	scriptGlobals.DUTRev = cfg.get('RX_SETUP','REVISION').split(",")
	scriptGlobals.DUTMfg = cfg.get('RX_SETUP','MANUFACTURE_NAME').split(",")
	scriptGlobals.DUTPackage = cfg.get('RX_SETUP','PACKAGE').split(",")
	scriptGlobals.DUTRefFreqSrc = cfg.get('RX_SETUP','EXT_CLK_SRC').split(",")
	scriptGlobals.DUTTemperature = cfg.get('RX_SETUP','TEMPERATURE').split(",")
	scriptGlobals.DUTTemperatureUnit = cfg.get('RX_SETUP','TEMPERATURE_UNIT').split(",")
	scriptGlobals.DUTVoltage = cfg.get('RX_SETUP','VOLTAGE').split(",")
	# Create directory for log files
	Now = time.localtime(time.time())
	scriptGlobals.TestRun = time.strftime("%m/%d/%Y %H:%M:%S", Now)
	scriptGlobals.StartTime = scriptGlobals.TestRun
	# Save log
	scriptGlobals.TestResultsDirectory = scriptGlobals.BaseLogDirectory + "\\" + time.strftime("%m%d%Y_%H%M%S", Now) + "\\"
	logDirectory = scriptGlobals.TestResultsDirectory
	mainFrame.SaveLogDirectory(scriptGlobals.TestResultsDirectory)
	
	# make a copy of the cfg file in the result dir for latter reference
	if (os.path.exists(scriptGlobals.TestResultsDirectory) != 1):
		nt.mkdir(scriptGlobals.TestResultsDirectory)
	shutil.copy(cfgFilePath, scriptGlobals.TestResultsDirectory)
	
	logFilesList = []
	sitesList = []
	ttffLogFileList = []
	logFileExtension = []
	
	pfIdx = 0
	for fExt in scriptGlobals.DutRxLogTypesList:
		if (fExt == "GPS"):
			logFileExtension.append(".gps")
		elif (fExt == "GP2"):
			logFileExtension.append(".gp2")
		else:
			logFileExtension.append(".txt")
	
	scriptGlobals.LogFileExtsList = logFileExtension
	# Open up host app if required
	portIdx = 0
	mainError = 0

	hostAppIdx = 0;
	waitForCodeLoad = False
	for port in portList:
		if (port < 0):
			portIdx = portIdx + 1
			continue
		if(hostAppRequired[portIdx] == 1):
			if(rxSourceDevice[portIdx] == "UART"):
				portName = "SW_COM" + str(port)						
			elif (rxSourceDevice[portIdx] == "TCP/IP_CLIENT"):
				portName = "SW_TCP" + str(port)
			elif (rxSourceDevice[portIdx] == "TCP/IP_SERVER"):
				portName = "SW_TCP" + str(inPort)			
			elif (rxSourceDevice[portIdx] == "PLAYBACK_FILE"):
				portName = "PlayBack" + str(inPort)
			elif (rxSourceDevice[portIdx] == "I2C"):
				portName = "I2C" + str(inPort)
			else:
				portName = "SW" + str(inPort)
			# portName = "com_" + str(port)
			if (os.path.exists(mainHostAppDir) != 1):
				print "Error: %s not exist" %(mainHostAppDir)
				mainError += 1
				General.clsGlobal.ScriptError = True
				sys.exit()
			nt.chdir(mainHostAppDir)
			if(os.path.exists(portName) != 1):
				nt.mkdir(portName)
			nt.chdir(portName)
			if(len(mainHostAppFile) > hostAppIdx):
				if(mainHostAppFile[hostAppIdx] == "-1"):
					continue
				srcFilePath = mainHostAppDir + "\\" + mainHostAppFile[hostAppIdx]
				dstFilePath = ".\\" + mainHostAppFile[hostAppIdx]
				shutil.copyfile(srcFilePath, dstFilePath)
				hostAppDirs.append(mainHostAppDir + "\\" + portName + "\\" + mainHostAppFile[hostAppIdx])				
			else:
				srcFilePath = mainHostAppDir + "\\" + mainHostAppFile[-1]
				dstFilePath = ".\\" + mainHostAppFile[-1]
				shutil.copyfile(srcFilePath, dstFilePath)
				hostAppDirs.append(mainHostAppDir + "\\" + portName + "\\" + mainHostAppFile[hostAppIdx])
				
			if (scriptGlobals.DutFamilyList[portIdx] == "GSD3tw"):		
				if(len(hostAppExtraArgvs) > hostAppIdx):
					hostAppArgvs.append("-t" + str(trackerPortList[portIdx]) + " -a" + str(hostPortList[portIdx]) + " -r" + str(resetPortList[portIdx]) + " " + hostAppExtraArgvs[hostAppIdx])
				else:
					if(len(hostAppExtraArgvs) > 0):
						hostAppArgvs.append("-t" + str(trackerPortList[portIdx]) + " -a" + str(hostPortList[portIdx]) + " -r" + str(resetPortList[portIdx]) + " " + hostAppExtraArgvs[-1])
					
			else:
				if (scriptGlobals.DutFamilyList[portIdx] == "GSD4t"):
					tmpAppParams = Communication.CommunicationManager.CreateHostAppParams() 
										
					tmpAppParams.SiRFLiveInterfacePortName = portName
					tmpAppParams.TrackerPort = "COM" +str(trackerPortList[portIdx])					
					tmpAppParams.BaudRate = rxBaudList[portIdx];
					tmpAppParams.ResetPort = "COM" + str(resetPortList[portIdx]) 
					# tmpAppParams.TrackerPortSelect = comm.TrackerPortSelect;
					# tmpAppParams.WarmupDelay = comm.WarmupDelay;
					# tmpAppParams.OnOffLineUsage = comm.OnOffLineUsage
					# tmpAppParams.ExtSResetNLineUsage = comm.ExtSResetNLineUsage
					# tmpAppParams.DebugSettings = comm.DebugSettings
					tmpAppParams.LNAType = rxLNAType[portIdx]
					tmpAppParams.LDOMode = rxLDOMode[portIdx]
					tmpAppParams.DefaultTCXOFreq = rxExtClkFreq[portIdx]
					tmpAppParams.HostSWFilePath = hostAppDirs[-1]
					
					GUI.Commmunication.frmCommSettings.CreateGSD4tConfigFile(hostAppDirs[-1],tmpAppParams);
					
					tmpArgvStr = ""
					tmpCfgFilepath = " -y\"" + tmpAppParams.HostAppCfgFilePath + "\" "
					if(rxSourceDevice[portIdx] == "UART"):
						tmpAppParams.SiRFLiveInterfacePortName = "COM" + str(portList[portIdx])
						if(len(hostAppExtraArgvs) > hostAppIdx):
							if (hostAppExtraArgvs[hostAppIdx] != "-1"):
								tmpArgvStr = " " + hostAppExtraArgvs[hostAppIdx]							
						else:
							if (hostAppExtraArgvs[-1] != "-1"):
								tmpArgvStr = " " + hostAppExtraArgvs[-1]
						hostAppArgvs.append(" -a\\\\.\\COM" + str(hostPortList[portIdx]) + tmpCfgFilepath + tmpArgvStr)
					else:
						if (rxSourceDevice[portIdx] == "TCP/IP_CLIENT"):
							tmpAppParams.SiRFLiveInterfacePortName = "TCP" + str(portList[portIdx])	
							if(len(hostAppExtraArgvs) > hostAppIdx):
								if (hostAppExtraArgvs[hostAppIdx] != "-1"):
									tmpArgvStr = " " + hostAppExtraArgvs[hostAppIdx]							
							else:
								if (hostAppExtraArgvs[-1] != "-1"):
									tmpArgvStr = " " + hostAppExtraArgvs[-1]
							hostAppArgvs.append(" -n" + str(port) + tmpCfgFilepath + tmpArgvStr)
			
			# Open the host app we just added in the previous step
			hostStartWaitTime = 5			
			if(mainFrame.HostAppCtrl.IsExistingWin(hostAppDirs[-1]) == 0):
				mainFrame.HostAppCtrl.OpenWin("\"" + hostAppDirs[-1]+ "\"", hostAppArgvs[-1])
				waitForCodeLoad = True
				print "Wait %ds to start next host" % (hostStartWaitTime)
				time.sleep(hostStartWaitTime)
			hostAppIdx = hostAppIdx + 1
			
		portIdx = portIdx + 1
	scriptGlobals.HostAppDirectory = hostAppDirs
	scriptGlobals.HostAppArgumentsList = hostAppArgvs
	# copy patch files
	patchIdx = 0;
	portIdx = 0;
	for port in portList:
		if (port < 0):
			portIdx = portIdx + 1
			continue
		if(patchRequired[portIdx] == 1):
			portName = "com_" + str(port)
			nt.chdir(mainHostAppDir)
			if(os.path.exists(portName) != 1):
				nt.mkdir(portName)
			nt.chdir(portName)
			if(len(patchFiles) > patchIdx):
				if(patchFiles[patchIdx] == "-1"):
					continue
				srcFilePath = mainHostAppDir + "\\" + patchFiles[patchIdx]
				dstFilePath = ".\\NVM2" 
				# dstFilePath = ".\\" + patchFiles[patchIdx]
				shutil.copyfile(srcFilePath, dstFilePath)								
			else:
				srcFilePath = mainHostAppDir + "\\" + patchFiles[-1]
				dstFilePath = ".\\NVM2" 
				# dstFilePath = ".\\" + patchFiles[-1]
				shutil.copyfile(srcFilePath, dstFilePath)			
				
			patchIdx = patchIdx + 1
			
		portIdx = portIdx + 1
		
	nt.chdir(currentDir)
	if(os.path.exists(logDirectory) != 1):
		nt.mkdir(logDirectory)
	
	# waitForCodeLoad = False
	# last element is timeout in ms
	hostWaitTime = 60
	msgString = "Wait %d seconds for host to load?" % (hostWaitTime)
	if (waitForCodeLoad == True):	
		result = MessageBoxEx.Show(msgString, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,10000);
		
		if (result == DialogResult.Yes):
			print "%s: Wait %ds for host to load bin file" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),hostWaitTime)
			mainFrame.Delay(hostWaitTime)
		else:
			print "Skip host waiting"	

	Now = time.localtime(time.time())
	timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)	
	
	pythonWin = mainFrame.GetPythonWindowRef()
	pythonWin.PythonEngineOutput.LogFilePath = logDirectory + "scriptExecution_" +timeNowStr +".log"
	pythonWin.PythonEngineOutput.OpenFile()
	scriptGlobals.Console = pythonWin
		
	print "Done reading global configuration."
	mainFrame.SetScriptDone(True)
	
	#mainFrame.RunAutoTest()

except:
	nt.chdir(currentDir)
	scriptError = sys.exc_info()
	if (General.clsGlobal.Abort == False):
		if (len(scriptError) >= 3):
			trackBack = scriptError[2]
			scriptErrorMessage = scriptError[1]
			lineno = trackBack.tb_lineno
			print "Error Message: %s -- Line: %d" % (scriptErrorMessage,lineno)
		print "Error running script: %s" %(scriptGlobals.ScriptName)
	    
	sys.exc_clear()
	scriptGlobals.MainFrame.SetScriptDone(True)  





