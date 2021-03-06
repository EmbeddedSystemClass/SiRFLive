from System.Threading import Thread
from System import Array
import time
import random

import scriptGlobals
import scriptUtilities

def logResults():
    global testPortList
    global failCountList    
    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	
	if (testPortList[comIdx] == 0):
	    comIdx = comIdx + 1
	    continue
	if (myPort.comm.m_NavData.Valid == True):
	    aidedTTFF = float(myPort.comm.RxCtrl.RxTTFF)	
	    pos2D = float(myPort.comm.RxCtrl.RxFirstFix2DPositionError)	
	    print "%s: TTFF: %.1f, Accuracy: %.2f" % (myPort.comm.PortName, aidedTTFF,pos2D)
	else:
	    aidedTTFF = -9999
	    pos2D = -9999
	    print "%s: Timeout" % (myPort.comm.PortName)
	    
	if ( aidedTTFF < 0):
	    failCountList[comIdx] = failCountList[comIdx] + 1
	elif (aidedTTFF > 20):
	    failCountList[comIdx] = failCountList[comIdx] + 1
	elif (pos2D < 0):
	    failCountList[comIdx] = failCountList[comIdx] + 1
	elif (pos2D > 100):
	    failCountList[comIdx] = failCountList[comIdx] + 1
	else:
	    failCountList[comIdx] = failCountList[comIdx]
	comIdx = comIdx + 1
	    
def checkTestProgress():
    global testPortList
    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	if (testPortList[comIdx] == 1):
	    return True
	comIdx = comIdx + 1
    return False

def writeSummaryFile(testName,aidingType,margin,cycle):    
    global testPortListPassFail
    global sumFileHanlder
    global failCountList
    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	sumLogStr = "Test: %s --- Aiding Type: %s --- Margin: %d\n\t%s: %s -- %d/%d cycles\n" % (testName, aidingType, margin, myPort.comm.PortName, testPortListPassFail[comIdx], failCountList[comIdx],cycle)
	sumFileHanlder.write(sumLogStr)
	comIdx = comIdx + 1

def checkSim():
    if (mainFrame.SimCtrl.GetSimStatus() == "Ended"):
	return True
    else:
	return False

def setSimPower(test, atten):
    print "Test %d, atten = %.2f" % (test+1, atten)
    mainFrame.SimCtrl.SetPowerLevel("-",0,0,True,True,True)
    mainFrame.Delay(1)
    MAX_CHAN = 8
    print "Set power to 0"
    if (test == 0):
	# test # 1	
	randChan = random.randint(0,MAX_CHAN-1)
	mainFrame.SimCtrl.SetPowerLevel("-",-12+atten,randChan,True,False,True)
	print "random channel: %d" %(randChan)
	for restChan in range(0,MAX_CHAN):
	    if (restChan == randChan):
		continue
	    mainFrame.Delay(1)
	    mainFrame.SimCtrl.SetPowerLevel("-",-17+atten,restChan,True,False,True) 
    elif (test == 2):
	randChan1 = random.randint(0,MAX_CHAN-1)
	mainFrame.SimCtrl.SetPowerLevel("-",1+atten,randChan1,True,False,True)
	mainFrame.Delay(1)
	randChan2 = randChan1
	while(randChan2 == randChan1):
	    randChan2 = random.randint(0,MAX_CHAN-1)
	mainFrame.SimCtrl.SetPowerLevel("-",-5+atten,randChan2,True,False,True)
	for restChan in range(0,MAX_CHAN):
	    if ((restChan == randChan2) or (restChan == randChan1)):
		continue
	    mainFrame.Delay(1)
	    mainFrame.SimCtrl.SetPowerLevel("-",-17+atten,restChan,True,False,True) 
    elif (test == 3):
	mainFrame.SimCtrl.SetPowerLevel("-",atten,0,True,False,True)
	mainFrame.SimCtrl.SetPowerLevel("-",atten,1,True,False,True)
	mainFrame.SimCtrl.SetPowerLevel("-",atten,2,True,False,True)
	mainFrame.SimCtrl.SetPowerLevel("-",atten,6,True,False,True)
	mainFrame.SimCtrl.SetPowerLevel("-",atten,7,True,False,True)

	atten1 = -6 + atten
	mainFrame.SimCtrl.SetPowerLevel("-",atten1,3,True,False,True)
	# mainFrame.Delay(1)
	mainFrame.SimCtrl.SetPowerLevel("-",atten1,4,True,False,True)
	# mainFrame.Delay(1)
	mainFrame.SimCtrl.SetPowerLevel("-",atten1,5,True,False,True)
	
	# turn off chans 9,10,11,12
	mainFrame.SimCtrl.SetPowerOnOff("-", 8,False,True,False)
	mainFrame.SimCtrl.SetPowerOnOff("-", 9,False,True,False)
	mainFrame.SimCtrl.SetPowerOnOff("-", 10,False,True,False)
	mainFrame.SimCtrl.SetPowerOnOff("-", 11,False,True,False)
    else:
	mainFrame.SimCtrl.SetPowerLevel("-",atten,0,True,True,True)
  
    # mainFrame.Delay(1)
    # drop atten
    # scriptUtilities.logApp("*", "Set desired signal level")
    # mainFrame.SimCtrl.SetPowerLevel("-",atten,0,True,True,False)
    mainFrame.Delay(2)

def geodeticNavigationDataHdlr(* args):

    doWorkArg = args[1] 

    try:
	# myQ.EndReceive(asyncResult.AsyncResult)	
	myQ = Utilities.ListenerManager.Marshall(doWorkArg)	
	myComName = myQ.MessageCommMgr
        
	myCom = mainFrame.GetPortRefByPortName(myComName)
	if (myCom):	    
	    myCom.comm.RxCtrl.SSBP2PAccuracy()	    
	    
    except:
	if (exiting == False):	    
	    print "TIA916 test encounters error in geodeticNavigationDataHdlr handler function"  


#main start here
try: 
    mainFrame.SetScriptDone(False)
    General.clsGlobal.Abort = False
    General.clsGlobal.AbortSingle = False
    General.clsGlobal.ScriptError = False
    scriptGlobals.Exiting = False
    scriptGlobals.TestAborted = False
    
    # print "Running: %s" % (scriptGlobals.ScriptName)    
    scriptUtilities.readNSaveTestConfig()  
    
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()
    sCfg.read(scriptGlobals.ScriptConfigFilePath)
    
    # timeout = 26
    General.clsGlobal.LoopitTimeout = int(scriptUtilities.ConvertToDouble(sCfg.get('TEST_PARAM','TIMEOUT')))
    General.clsGlobal.LoopitIteration = sCfg.getint('TEST_PARAM','NUMBER_OF_LOOP') 
    General.clsGlobal.LoopitResetType = sCfg.get('TEST_PARAM','RESET_TYPE')
    scriptGlobals.TTFFTimeout= General.clsGlobal.LoopitTimeout
    earlyCompletionFlag = sCfg.getint('TEST_PARAM','ALLOW_EARLY_TERMINATION')
    if (earlyCompletionFlag == 1):
	General.clsGlobal.LoopitEarlyTermination = True
    else:
	General.clsGlobal.LoopitEarlyTermination = False
	
    # #scriptGlobals.MainFrame.AutoTestAbortHdlr.SetSiRFLiveEventHdlr += scriptUtilities.abortTest    
    baseTestName = scriptGlobals.TestName
    failCountList = []
    testPortListPassFail = []
       
    Now = time.localtime(time.time())
    timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
    
    scriptFullName = os.path.basename(mainFrame.TestScriptPath)

    scriptAbsPath = os.path.dirname(mainFrame.TestScriptPath)
    scriptGlobals.ScriptName = scriptFullName .split(".")[0]    
    scriptGlobals.ScriptConfigFilePath = scriptAbsPath + "\\" + scriptGlobals.ScriptName + ".cfg";
    test5TruthDataFilePath = scriptAbsPath + "\\test5_truth.csv";    
        
    sumFilePath = scriptGlobals.TestResultsDirectory + "\\" + scriptGlobals.ScriptName + ".log"
    sumFileHanlder = open(sumFilePath,"w")
    
    shutil.copyfile(scriptGlobals.ScriptConfigFilePath, scriptGlobals.TestResultsDirectory + "\\" + scriptGlobals.ScriptName + ".cfg")
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()
    sCfg.read(scriptGlobals.ScriptConfigFilePath)
    # override SIM address
    mainFrame.SimCtrl.IPAddress =  sCfg.get('SIM','SIM_ADDRESS')
    scriptGlobals.SimAddr = mainFrame.SimCtrl.IPAddress
    # override SIM Port
    mainFrame.SimCtrl.Port =  sCfg.get('SIM','SIM_PORT')
    scriptGlobals.SimPort = mainFrame.SimCtrl.Port
    
    # always run with sim 
    scriptGlobals.SignalSource = General.clsGlobal.SIM
    
    if (mainFrame.SimCtrl.IPAddress == "127.0.0.1"):
	# Set registry key
	mainFrame.SimCtrl.SpirentRegKey()
	# Run SimPLEX
	mainFrame.Delay(1)
	mainFrame.SimCtrl.RunSimPLEX()
	
	print "Wait 30s for SimPLEX to load ... "
	mainFrame.Delay(30)
						  
    mainFrame.SimCtrl.SetTriggerMode(DeviceControl.Simplex.SimTriggerMode.Software)
    mainFrame.SimCtrl.PopUps(False)
          
    test1To4EphFiles = "\\ValidTIA916_brdc2270.ai3eph"
    test1To4AcqAssistFiles = "\\TIA_ACQ.csv"
    
    mainSimFilesDirs = "C:\\Program Files\\Spirent Communications\\SimPLEX\\Scenarios\\TIA916"
    test1To5SimFilesDirs = ["\\GPS_Sensitivity_wfs\\SimPLEX_GPS_Sensitivity_wfs\\GPS_Sensitivity_wfs.sim",
			    "\\GPS_Accuracy_wfs\\SimPLEX_GPS_Accuracy_wfs\\GPS_Accuracy_wfs.sim",
			    "\\GPS_Dynamic_Range_wfs\\SimPLEX_GPS_Dynamic_Range_wfs\\GPS_Dynamic_Range_wfs.sim",
			    "\\GPS_Multipath_wfs\\SimPLEX_GPS_Multipath_wfs\\GPS_Multipath_wfs.sim",
			    "\\GPS_Moving_Scenario_wfs\SimPLEX_GPS_Moving_Scenario_wfs\\GPS_Moving_Scenario_wfs.sim"]
    
    test5SimFile = mainSimFilesDirs + test1To5SimFilesDirs[4]
    referenceSimFile = mainSimFilesDirs + test1To5SimFilesDirs[1]
    
    simFilesWeekNum = [103,103,103,103,103]
    simFilesStartTOW = [213544,213544,213544,213544,213544]
    # get enable test
    testLoops = [];
    test1_enable = sCfg.getint('TESTS','SENSITIVITY_TEST1')
    testLoops.append(test1_enable)
    test2_enable = sCfg.getint('TESTS','NOMINAL_ACCURACY_TEST2')
    testLoops.append(test2_enable)
    test3_enable = sCfg.getint('TESTS','DYNAMIC_RANGE_TEST3')
    testLoops.append(test3_enable)
    test4_enable = sCfg.getint('TESTS','MULTIPATH_TEST4')
    testLoops.append(test4_enable)
    test5_enable = sCfg.getint('TESTS','MOVING_SCENARIO_TEST5')
    testLoops.append(test5_enable)
    
    MAX_SESSION_TEST = 6
    
    # get number of cycles for each test
    testCycles = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "CYCLES_TEST" + `i`
	test_cycle = sCfg.getint('TEST_SETUP',myKey)
	testCycles.append(test_cycle)
    # print testCycles
    
    # get signal atten
    signalAttens = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "SIGNAL_ATTN_TEST" + `i`
	test_signalAtten = sCfg.getfloat('TEST_SETUP',myKey)
	signalAttens.append(test_signalAtten)
    
    # print signalAttens    
       
    scriptGlobals.CableLoss = sCfg.getfloat('TEST_SETUP','CABLE_LOSS') 
    print "Cableloss: %.2f" % (scriptGlobals.CableLoss)
    
    # get aiding test type
    aidingTypes = []	
    test1_enable = sCfg.getint('TEST_SETUP','MS_BASED_TEST')
    aidingTypes.append(test1_enable)
    test2_enable = sCfg.getint('TEST_SETUP','MS_ASSIST1_TEST')
    aidingTypes.append(test2_enable)
    test3_enable = sCfg.getint('TEST_SETUP','MS_ASSIST2_TEST')
    aidingTypes.append(test3_enable)
    test4_enable = sCfg.getint('TEST_SETUP','MSAB_B_TEST')
    aidingTypes.append(test4_enable)
    test5_enable = sCfg.getint('TEST_SETUP','MSAB_A1_TEST')
    aidingTypes.append(test5_enable)
    test6_enable = sCfg.getint('TEST_SETUP','MSAB_A2_TEST')
    aidingTypes.append(test6_enable)
    
    print aidingTypes
    
    msbMargin = sCfg.get('TEST_SETUP','MS_BASED_MARGIN').split(",")
    msa1Margin = sCfg.get('TEST_SETUP','MS_ASSIST1_MARGIN').split(",")
    msa2Margin = sCfg.get('TEST_SETUP','MS_ASSIST2_MARGIN').split(",")
    msab_bMargin = sCfg.get('TEST_SETUP','MSAB_B_MARGIN').split(",")
    msab_a1Margin = sCfg.get('TEST_SETUP','MSAB_A1_MARGIN').split(",")
    msab_a2Margin = sCfg.get('TEST_SETUP','MSAB_A2_MARGIN').split(",")
    # get freq accuracy
    relFreqAccs = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "REL_FREQ_ACC_TEST" + `i`
	test_refFreqAcc = sCfg.getfloat('AIDING_PARAMS',myKey)
	relFreqAccs.append(test_refFreqAcc)
    
    print relFreqAccs
    
    # get Hrz QoS
    hrzQoSSettings = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "HRZ_QOS_TEST" + `i`
	test_hrzQoS = sCfg.getfloat('AIDING_PARAMS',myKey)
	hrzQoSSettings.append(test_hrzQoS)
    
    print hrzQoSSettings
    
    # get Vrt QoS
    vrtQoSSettings = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "VRT_QOS_TEST" + `i`
	test_vrtQoS = sCfg.getfloat('AIDING_PARAMS',myKey)
	vrtQoSSettings.append(test_vrtQoS)
    
    print vrtQoSSettings
    
    # get response time max
    respTimeMaxs = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "RESPONSE_TIME_MAX_TEST" + `i`
	test_respTimeMax = sCfg.getint('AIDING_PARAMS',myKey)
	respTimeMaxs.append(test_respTimeMax)
    
    print respTimeMaxs
    
    # get priority 
    priorities = []
    for i in range(1,MAX_SESSION_TEST):
	myKey = "PRIORITY_TEST" + `i`
	test_priority = sCfg.get('AIDING_PARAMS',myKey)
	if (test_priority == "Response Time"):
	    priorities.append(1)
	elif (test_priority == "Position Error"):
	    priorities.append(2)
	elif (test_priority == "Use Entire Response Time"):
	    priorities.append(3)
	else:
	    priorities.append(0)
    
    print priorities
    
    strongestSignalLevel = [21,40,40,40,40]
    
    # get frequency transfer method
    freqTransferMethod = sCfg.get('AIDING_PARAMS','FREQ_TRANFER_METHOD')
    # print freqTransferMethod   
    
    scriptGlobals.getTTFFs = []
    scriptGlobals.getNavs = []
    # setup each active com ports    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue 
	myPort = scriptGlobals.DutPortManagersList[comIdx]

	# myPort.comm.OpenPort()
	myPort.AutoTestCloseMicsWindows()
	# reset logfile name
	myPort.comm.Log.SetDurationLoggingStatusLabel("")
	if (myPort.comm.IsSourceDeviceOpen() == False):
	    if (myPort.comm.OpenPort() == False):
		scriptGlobals.DutMainPortsList[comIdx] = -1
		comIdx = comIdx + 1 
		continue
		
	myPort.RunAsyncProcess()
	myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)
	myPort.comm.RxCtrl.UTCOffset = 13
	myPort.comm.RxCtrl.PollSWVersion()
	if (myPort.SignalViewLocation.IsOpen == False):
	    mainFrame.CreateSignalViewWin(myPort)
	if (myPort.DebugViewLocation.IsOpen == False):
	    mainFrame.CreateDebugViewWin(myPort)

	myPort.comm.RxCtrl.PollSWVersion()
	
	failCountList.append(0)
	testPortListPassFail.append("Fail")	

	# intialize flags to indicate RX get TTFF
	scriptGlobals.getTTFFs.append(False)
	scriptGlobals.getNavs.append(False)	
	
	comIdx = comIdx + 1 
    mainFrame.UpdateGUIFromScript()
    testName = ["SENSITIVITY_TEST1", "NOMINAL_ACCURACY_TEST2", "DYNAMIC_RANGE_TEST3", "MULTIPATH_TEST4", "MOVING_SCENARIO_TEST5"]
    aidingName = ["MSB", "MSA1", "MSA2", "MSAB_B", "MSAB_A1", "MSAB_A2"]
    
    #Initialize
    print "Initializing ..."
    #mainFrame.SimCtrl.EndScenario(False)
    #mainFrame.Delay(5)
    
    scriptUtilities.init()
    SPAzAtten = 20
    print "Bring SPAz down %d dB" % (SPAzAtten)
    scriptGlobals.MainFrame.SpazCtrl.WriteSPAzAtten(SPAzAtten)
    simFilePath =  mainSimFilesDirs + test1To5SimFilesDirs[1]
    logString = "Loading: " + simFilePath
    scriptUtilities.logApp("*", logString)
    mainFrame.SimCtrl.EndScenario(False)
    mainFrame.SimCtrl.SelectScenario(simFilePath)
    mainFrame.Delay(5)
    mainFrame.SimCtrl.RunScenario()
    # wait for nav
    waitTTFFLoops = 0
    while(waitTTFFLoops < 13):
	if(scriptUtilities.waitForNav() == True):
	    break
	else:
	    mainFrame.Delay(10)
	waitTTFFLoops = waitTTFFLoops + 1 
    
    mainFrame.SimCtrl.EndScenario(False)
    comIdx = 0 
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue 
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# open TTB port and send aiding if TTB aiding is enable
	myTTBPort = scriptGlobals.DutTTBPortsList[comIdx]
	if ( myTTBPort > 0):			
	    print "Port %s connect TTB port %d" % (myPort.comm.PortName, myTTBPort)
	    myPort.comm.OpenTTBPort(myTTBPort)    
	    
	comIdx = comIdx + 1	
    # loop through test 
    # loop through aiding type 
    # test 5 only need to run once, no aiding
    test5Run = False
    for aidingTypeIndex in range (0, len(aidingTypes)):
	if (scriptGlobals.TestAborted == True):
	    break
	if (aidingTypes[aidingTypeIndex] == 0):
	    print "%s disable" % (aidingName[aidingTypeIndex])
	    continue
	if (aidingTypeIndex == 0):
	    marginLoop = msbMargin
	elif (aidingTypeIndex == 1):
	    marginLoop = msa1Margin
	elif (aidingTypeIndex == 2):
	    marginLoop = msa2Margin
	elif (aidingTypeIndex == 3):
	    marginLoop = msab_bMargin
	elif (aidingTypeIndex == 4):
	    marginLoop = msab_a1Margin
	elif (aidingTypeIndex == 5):
	    marginLoop = msab_a2Margin
	else:
	    marginLoop = msbMargin
	
	if (len(marginLoop) == 0):
	    marginLoop = [0]
	
	for test in range(0, len(testLoops)):
	    # loop through margin
	    # print marginLoop
	    marginIdx = 0
	    for marginStr in marginLoop:
		if (scriptGlobals.TestAborted == True):
		    break
		# for test in range(0, len(testLoops)):
		margin = float(marginStr);
		if (test > 0):
		    if (marginIdx == 0):
			marginIdx = marginIdx + 1
			margin = 0.0
		    else:
			continue
		if (margin < 0):
		    atten = scriptGlobals.CableLoss - signalAttens[test]
		else:
		    atten = scriptGlobals.CableLoss - margin 
		if (testLoops[test] == 0):
		    print "%s disable" % (testName[test])
		    continue			
		    
		simFilePath =  mainSimFilesDirs + test1To5SimFilesDirs[test]
		logString = "Loading: " + simFilePath
		scriptUtilities.logApp("*", logString)
		mainFrame.SimCtrl.EndScenario(False)
		mainFrame.SimCtrl.SelectScenario(simFilePath)
		mainFrame.Delay(5)
		if (test == 0):
		    # Sensitivity test set margin
		    for chan in range(0,4):			
			mainFrame.SimCtrl.SetPowerLevel("-",-19+atten,chan,True,False,True)
		    # change SPAz
		    if (SPAzAtten != 0):
			SPAzAtten = 0
			print "Bring SPAz atten to %d dB" % (SPAzAtten)
			scriptGlobals.MainFrame.SpazCtrl.WriteSPAzAtten(SPAzAtten)
		else:
		    scriptUtilities.setAtten(atten)
		    # change SPAz
		    if (SPAzAtten == 0):
			SPAzAtten = 20
			print "Bring SPAz atten to %d dB" % (SPAzAtten)
			scriptGlobals.MainFrame.SpazCtrl.WriteSPAzAtten(SPAzAtten)
		    
		mainFrame.SimCtrl.RunScenario()	
		
		currentTestRun = "%s %s Margin %s" % (aidingName[aidingTypeIndex],testName[test], marginStr)	
		General.clsGlobal.CurrentRunningTest = currentTestRun
		print "Current Test: %s" %(currentTestRun)		
		
		comIdx = 0 
		for usePort in scriptGlobals.DutMainPortsList:
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue 
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    myPort.comm.RxCtrl.PollSWVersion()
		    # open TTB port and send aiding if TTB aiding is enable
		    myTTBPort = scriptGlobals.DutTTBPortsList[comIdx]
		    if ( myTTBPort > 0):			
			myPort.comm.SendTTBReset(General.clsGlobal.COLD)
			
		    comIdx = comIdx + 1	
		
		ttbWaitTime = 20
		print "Wait for TTB nav ..." 
		mainFrame.Delay(20)
		TTBNavStatus = False
		count = 0
		while((TTBNavStatus == False) and (count < ttbWaitTime)):
		    TTBNavStatus = scriptUtilities.waitForTTBNav()
		    count = count + 1
		    print "Wait for TTB nav loop %d" %(count)
		    if ((TTBNavStatus == True) or (count >= ttbWaitTime)): 		
			break
		    if ((General.clsGlobal.Abort == True) or (General.clsGlobal.AbortSingle == True)):
			print "Test aborted"
			break	
		    mainFrame.Delay(10)
		
		if (TTBNavStatus == False):
		    print "TTB(s) timeout waiting for Nav"
		    continue
		
		ttbWaitTime = 120
		print "Wait %d for TTB to stablize ..."  %(ttbWaitTime)
		mainFrame.Delay(ttbWaitTime)
		
		comIdx = 0 
		for usePort in scriptGlobals.DutMainPortsList:
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue 
		    myPort = scriptGlobals.DutPortManagersList[comIdx]		        
		    myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)
		    myPort.comm.RxCtrl.UTCOffset = 13
		    myPort.comm.RxCtrl.PollSWVersion()
		    myPort.comm.SendTTBAiding()
		    # Create directory for log files		    
		    if (test < 4):		    		
	    		
			if (myPort.comm.AutoReplyCtrl.HWCfgCtrl.FreqAidMethod == 0):
			    freqTxStr = "Counter";
			else:
			    freqTxStr = "Non_Counter";			
			
			testSetupMarginString = `margin`.replace('.','p')
			baseName = "%s%s_%s_%s_%s_%s_%s_%s" % (scriptGlobals.TestResultsDirectory,timeNowStr , scriptGlobals.DutNamesList[comIdx], myPort.comm.PortName, freqTxStr, aidingName[aidingTypeIndex],testName[test] ,testSetupMarginString)
			
			portLogFile = baseName + scriptGlobals.LogFileExtsList[comIdx]
			ttffLogFile = baseName + "_ttff.csv"
			
			print portLogFile
			print ttffLogFile
		    
		    # myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_ExtEph_fromFile = True
		    myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromFile = True
		    if (aidingTypeIndex == 0): # MSB
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 1;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = False
		    elif (aidingTypeIndex == 1): #MSA1
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 0;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = True
		    elif (aidingTypeIndex == 2): #MSA2
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 0;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = True
		    elif (aidingTypeIndex == 3): #MSAB-MSB
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 4;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = False
		    elif (aidingTypeIndex == 4): #MSAB-MSA1
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 4;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = True
		    elif (aidingTypeIndex == 5): #MSAB-MSA2
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 4;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = True
		    else:
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 1;
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = False
			
		    myPort.comm.AutoReplyCtrl.FreqTransferCtrl.DefaultFreqIndex = scriptGlobals.DutDefaultClkIndicesList[comIdx]	    
		    myPort.comm.AutoReplyCtrl.AidingProtocolVersion = scriptGlobals.DutAi3ICDsList[comIdx]
		    myPort.comm.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax = int(hrzQoSSettings[test])
		    myPort.comm.AutoReplyCtrl.PositionRequestCtrl.VertErrMax = int(vrtQoSSettings[test])
		    myPort.comm.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax = int(respTimeMaxs[test])
		    myPort.comm.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority = int(priorities[test])
		    myPort.comm.AutoReplyCtrl.FreqTransferCtrl.Accuracy = relFreqAccs[test]
		    
		    # modified per test parameters		    
		    testSetupLevelString = "%s" %(strongestSignalLevel[test]) 	
		    scriptGlobals.TestName = "%s-%s-%s-%s" %(baseTestName,freqTxStr.replace('_','-'),aidingName[aidingTypeIndex].replace('_','-'),testName[test].replace('_','-'))
		    scriptGlobals.TestID = "%s-%sdBHz-Margin%sdB" % (scriptGlobals.TestName,testSetupLevelString,testSetupMarginString)	 
		    Now = time.localtime(time.time())
		    scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
		    myPort.comm.RxCtrl.DutStationSetup.SignalLevel = strongestSignalLevel[test]
		    scriptGlobals.SignalMargin = margin
		    scriptUtilities.updateDUTInfo(comIdx)
		    myPort.comm.m_TestSetup.Atten = atten		
		    # Set limit
		    myPort.comm.RxCtrl.DutStationSetup.TTFFLimit = 16		    
		    if (test == 0) or (test == 3):	
			# Sensitivity and multipath
			myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = 60
		    elif (test == 1):
			# Normal Accuracy
			myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = 25
		    elif (test == 2):
			# Dynamic Range
			myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = 50
		    elif(test == 3):
			myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = 60
		    elif (test == 4):
			# Moving
			myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = 35
		    else:
			myPort.comm.RxCtrl.DutStationSetup.HrzErrorLimit = 60
		    # Set debug level ACQ level 3
		    #myPort.comm.RxCtrl.SendRaw("A0A20033E400010101010101010101010101010101010501010101010101010101010101010101010101010101010101010101010101010119B0B3") 
		    
		    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = True
		    #open log file
		    if (test < 4):			
			myPort.comm.RxCtrl.ResetCtrl.ResetCount = 0
			myPort.comm.Log.OpenFile(portLogFile);
			myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)		    
		    
		    #mark to test port
		    testPortList[comIdx] = 1
		    failCountList[comIdx] = 0		    
		    testPortListPassFail[comIdx] = "N/A"
		    
		    comIdx = comIdx + 1
		
		# test 5 no reset
		if (test < 4):		    
		    if (testCycles[test] <= 0):
			testSamples = 2766
		    else:
			testSamples = testCycles[test]
		    # maxErrorAllow = getMaxErrorAllow(testSamples)
		    comIdx = 0 
		    for usePort in scriptGlobals.DutMainPortsList:
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue 
			myPort = scriptGlobals.DutPortManagersList[comIdx]
					
			# set default
			myPort.comm.RxCtrl.ResetCtrl.ResetInterval = General.clsGlobal.LoopitTimeout
			myPort.comm.AutoReplyCtrl.AidingProtocolVersion = scriptGlobals.DutAi3ICDsList[comIdx]
						
			myPort.comm.AutoReplyCtrl.AutoReplyApproxPositionResp();
			if ((aidingTypeIndex == 2) or (aidingTypeIndex == 5)): # MSA2 no position aiding
			    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Reject = True				
			
			myPort.comm.AutoReplyCtrl.TimeTransferCtrl.Skew = 0   
			myPort.comm.AutoReplyCtrl.AutoReplyParams.UseTTB_ForTimeAid = True
			myPort.comm.AutoReplyCtrl.EphFilePath = scriptAbsPath + test1To4EphFiles
			myPort.comm.AutoReplyCtrl.AcqDataFilePath =  scriptAbsPath + test1To4AcqAssistFiles
			print "Eph file: %s" % (myPort.comm.AutoReplyCtrl.EphFilePath)
			myPort.comm.RxCtrl.RxNavData.ValidatePosition = True
			comIdx = comIdx + 1
		    # print "Max trials: %d -- Max Error Allow: %d" %(testSamples, maxErrorAllow)
		    for cycle in range(0,testSamples):
			if (scriptGlobals.TestAborted == True):
			    break
			
			randStartTime = random.randint(1,17)			
			logString = "Randomization wait %ds" % (randStartTime)
			scriptUtilities.logApp("*",logString)
			mainFrame.Delay(randStartTime)	
			
			testCycleString = "###### Session %d ######" % (cycle+1) 
			scriptUtilities.logApp("*",testCycleString)			
			
			comIdx = 0 
			for usePort in scriptGlobals.DutMainPortsList:
			    if (usePort < 0):
				comIdx = comIdx + 1
				continue 
			    myPort = scriptGlobals.DutPortManagersList[comIdx]
			    if (testPortList[comIdx] == 1):
				myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableEncryptedData = False;
				print "%s: Last clk drift = %.2f" % (myPort.comm.PortName, myPort.comm.LastClockDrift)
				print "%s: Last session clk drift = %.6f (%.6f)" % (myPort.comm.PortName,myPort.comm.LastAidingSessionReportedClockDrift * 1575.42,myPort.comm.LastAidingSessionReportedClockDrift)
			    
			    comIdx = comIdx + 1	
			
			#secIntoRun = mainFrame.SimCtrl.GetTimeIntoRun()
			#simTowNow = (simFilesStartTOW[test]*1.5 + int(secIntoRun)) * 100
			## do this quick so start it separately
			#comIdx = 0 
			#for usePort in scriptGlobals.DutMainPortsList:
			    #if (usePort < 0):
				#comIdx = comIdx + 1
				#continue 
			    #myPort = scriptGlobals.DutPortManagersList[comIdx]
			    #myPort.comm.RxCtrl.SetGPSTime(`simFilesWeekNum[test]`,`simTowNow`)
			    
			    #comIdx = comIdx + 1	
			
			#print "%s: SIM GPS TOW = %.6f" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),simTowNow)
			#mainFrame.Delay(10)
			# add 10 seconds so the Sim can start correctly		
			
			comIdx = 0 
			for usePort in scriptGlobals.DutMainPortsList:
			    if (usePort < 0):
				comIdx = comIdx + 1
				continue 
			    myPort = scriptGlobals.DutPortManagersList[comIdx]
			    # only reset when needed
			    if (testPortList[comIdx] == 1):				
				myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.COLD)
				
				# Disable static nav
				myPort.comm.WriteData("A0 A2 00 02 8F 00 00 8F B0 B3");
						    
			    comIdx = comIdx + 1
			    
			waitTTFFLoops = 0
			WAIT_TTFF_TIMEOUT = 6

			while(waitTTFFLoops < WAIT_TTFF_TIMEOUT):
			    if(scriptUtilities.checkTTFF() == True):
				break
			    else:
				mainFrame.Delay(5)
			    waitTTFFLoops = waitTTFFLoops + 1			
			
			#cleanup
			comIdx = 0
			for usePort in scriptGlobals.DutMainPortsList:
			    if (usePort < 0):
				comIdx = comIdx + 1
				continue 
			    myPort = scriptGlobals.DutPortManagersList[comIdx]
			    if (waitTTFFLoops >= WAIT_TTFF_TIMEOUT):
				# log result
				myPort.comm.RxCtrl.ResetCtrl.LogTTFFCsv()
			    
			    comIdx = comIdx + 1
			
			logResults()

			if (scriptGlobals.TestAborted == True):
			    break			
		    		    
		    #cleanup
		    comIdx = 0
		    for usePort in scriptGlobals.DutMainPortsList:
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue 
			myPort = scriptGlobals.DutPortManagersList[comIdx]	    

			myPort.comm.Log.CloseFile();
			myPort.comm.RxCtrl.ResetCtrl.CloseTTFFLog();
			myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False

			comIdx = comIdx + 1
		    
		    # write summary file    
		    writeSummaryFile(testName[test],aidingName[aidingTypeIndex],margin,cycle+1)
		    General.clsGlobal.DoneTests.Add(currentTestRun)
		    mainFrame.SimCtrl.EndScenario(False)
		else:
		    # test 5
		    if (test5Run == True):
			continue
		    test5Run = True
		    mainFrame.SimCtrl.EndScenario(False)
		    mainFrame.Delay(2)
		    scriptUtilities.init()
		    mainFrame.Delay(8)
		    comIdx = 0
		    for usePort in scriptGlobals.DutMainPortsList:
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue 
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			myPort.comm.ReadTruthData(test5TruthDataFilePath, 2)
			myPort.comm.RxCtrl.PollSWVersion()
			
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = True
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromFile = False
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = False
			myPort.comm.AutoReplyCtrl.EphFilePath = scriptAbsPath + test1To4EphFiles			
			
			myPort.comm.AutoReplyCtrl.FreqTransferCtrl.DefaultFreqIndex = scriptGlobals.DutDefaultClkIndicesList[comIdx]   
			myPort.comm.AutoReplyCtrl.AidingProtocolVersion = scriptGlobals.DutAi3ICDsList[comIdx]
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.NumFixes = 0
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.TimeBtwFixes = 2
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax = int(hrzQoSSettings[test])
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.VertErrMax = int(vrtQoSSettings[test])
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax = int(respTimeMaxs[test])
			myPort.comm.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority = int(priorities[test])
			myPort.comm.AutoReplyCtrl.FreqTransferCtrl.Accuracy = relFreqAccs[test]
			myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Reject = True
			myPort.comm.AutoReplyCtrl.HWCfgCtrl.FreqAidEnabled = False
			myPort.comm.AutoReplyCtrl.HWCfgCtrl.CoarseTimeEnabled = False;
			
			testSetupMarginString = `margin`.replace('.','p')
			baseName = "%s%s_%s_%s_%s_%s" % (scriptGlobals.TestResultsDirectory,timeNowStr , scriptGlobals.DutNamesList[comIdx], myPort.comm.PortName, testName[test] ,`margin`.replace('.','p'))			
			
			# modified per test parameters
			testSetupLevelString = "%s" %(strongestSignalLevel[test]) 
			scriptGlobals.TestName = "%s-%s" %(baseTestName,testName[test].replace('_','-'))
			scriptGlobals.TestID = "%s-%sdBHz-Margin%sdB" % (scriptGlobals.TestName,testSetupLevelString,testSetupMarginString)	 
			Now = time.localtime(time.time())
			scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
			myPort.comm.RxCtrl.DutStationSetup.SignalLevel = strongestSignalLevel[test]
			scriptGlobals.SignalMargin = margin
			scriptGlobals.TTFFLimit = 240
			scriptGlobals.TTFFTimeout = 240
			scriptUtilities.updateDUTInfo(comIdx)
			myPort.comm.m_TestSetup.Atten = atten
			myPort.comm.RxCtrl.DutStationSetup.TTFFLimit = scriptGlobals.TTFFLimit
			
		    	portLogFile = baseName + scriptGlobals.LogFileExtsList[comIdx]
			ttffLogFile = baseName + "_ttff.csv"			
			GeodeticNavigationDataLogFilePath = baseName + "_" + General.clsGlobal.P2PAccuracy + ".csv"
			GeodeticNavigationDataLogFile = myPort.comm.RxCtrl.CreateLog(General.clsGlobal.P2PAccuracy)
			
			myPort.comm.Log.SetDurationLoggingStatusLabel(portLogFile)
			myPort.comm.Log.OpenFile(portLogFile);

			GeodeticNavigationDataLogFile.OpenFile(GeodeticNavigationDataLogFilePath)
			myPort.comm.RxCtrl.LogWrite(General.clsGlobal.P2PAccuracy, myPort.comm.RxCtrl.ResetCtrl.GetTestSetup())
			
			print portLogFile
			print ttffLogFile
			print GeodeticNavigationDataLogFilePath			
						
			if (aidingTypeIndex == 0): # MSB
			    myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 1;
			    qName = General.clsGlobal.POSITION_RESPONSE_LABEL
			else:
			    scriptUtilities.logApp(myPort.comm.PortNum, "Not supported mode! -- Force using MSB");
			    myPort.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod = 1;
			    qName = General.clsGlobal.POSITION_RESPONSE_LABEL	
			
			q = myPort.comm.ListenersCtrl.Create(qName, myPort.comm.PortName)
			q.DoUserWork.DoWork  += geodeticNavigationDataHdlr
			
			myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = True
			# Set debug level ACQ level 3
			# myPort.comm.RxCtrl.SendRaw("A0A20033E400010101010101010101010101010101010501010101010101010101010101010101010101010101010101010101010101010119B0B3") 
			comIdx = comIdx + 1		    
		    
		    logString = "Loading: " + test5SimFile
		    scriptUtilities.logApp("*", logString)
		    mainFrame.SimCtrl.SelectScenario(test5SimFile)
		    mainFrame.Delay(1)
		    # drop atten
		    scriptUtilities.logApp("*", "Set desired signal level")
		    mainFrame.SimCtrl.SetPowerLevel("-",atten,1,True,True,False)
		    mainFrame.SimCtrl.RunScenario()
		    # sleep 10s so that we can set msg41 to output 2s
		    mainFrame.Delay(10)    
		    
		    comIdx = 0
		    for usePort in scriptGlobals.DutMainPortsList:
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue 
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			
			myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.COLD)			
			myPort.comm.ListenersCtrl.Start(General.clsGlobal.POSITION_RESPONSE_LABEL,myPort.comm.PortName)
						    
			# set message rate to 2 seconds
			# myPort.comm.WriteData("A0A2 0008 A6 00 29 02 00 00 00 00 00D1 B0B3")
			# myPort.comm.ListenersCtrl.Start(General.clsGlobal.GEODETIC_NAVIGATION_DATA_LABEL,myPort.comm.PortName)					    	
			
			comIdx = comIdx + 1
		    
		    # test 5 time 20 minutes / 5 sec  = 240
		    TEST5_DURATION = 240 
		    # TEST5_DURATION  = 10
		    print "%s: sleep %d sec" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),TEST5_DURATION*5)
		    waitLoops = 0

		    while(waitLoops < TEST5_DURATION):
			if(checkSim() == True):
			    break
			else:
			    mainFrame.Delay(5)
			waitLoops = waitLoops + 1
		    
		    #cleanup
		    comIdx = 0
		    for usePort in scriptGlobals.DutMainPortsList:
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue 
			myPort = scriptGlobals.DutPortManagersList[comIdx]	    
			# setup back to true for other tests
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = True
			myPort.comm.Log.CloseFile();
			myPort.comm.ListenersCtrl.Stop(General.clsGlobal.GEODETIC_NAVIGATION_DATA_LABEL,myPort.comm.PortName)
			myPort.comm.RxCtrl.LogCleanup()
			myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
			
			comIdx = comIdx + 1
		    
		    General.clsGlobal.DoneTests.Add(currentTestRun)
		    
    
    scriptGlobals.Exiting = True
    sumFileHanlder.close()    
    mainFrame.SetScriptDone(True) 
    myReport = mainFrame.ReportCtrl
    myReport.Percentile = "67,95"
    myReport.TTFFLimit = "16,16"
    myReport.HrErrLimit = "25,75"
    myReport.LimitVal = "16"
    myReport.TTFFReportType = 3
    myReport.TimeoutVal = `General.clsGlobal.LoopitTimeout`
    myReport.Summary_TIA916(logDirectory)
    #cleanup
    comIdx = 0
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue 
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
	comIdx = comIdx + 1
    print "TIA916 test completed" 

    mainFrame.UpdateGUIFromScript()
except:
    scriptUtilities.ExceptionHandler()


    
    
