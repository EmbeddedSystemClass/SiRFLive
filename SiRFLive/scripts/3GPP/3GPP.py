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

def getMaxErrorAllow(cycle):
    global numSamplesPass
    numError = 0
    
    if (cycle < 77):
	numError = 1
    else:
	for gi in range(0,len(numSamplesPass)):
	    if (gi == len(numSamplesPass) - 1):
		numError = gi
		break
	    if (cycle == numSamplesPass[gi]):
		numError =  gi + 1
		break
	    else:
		if ((cycle > numSamplesPass[gi]) and (cycle < numSamplesPass[gi+1])):
		    numError = gi + 1;
		    break
    return numError

def checkResults(cycle):    
    global testPortList
    global failCountList
    global numSamplesFail
    global numSamplesPass
    global testPortListPassFail
    
    comIdx = 0
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	if (testPortList[comIdx] == 0):
	    comIdx = comIdx + 1
	    continue
	if (failCountList[comIdx] >= maxErrorAllow):
	    testPortList[comIdx] = 0
	    testPortListPassFail[comIdx] = "Fail"
	    comIdx = comIdx + 1
	    continue
	if (failCountList[comIdx] < 6):
	    if (cycle >= numSamplesPass[failCountList[comIdx]]):
		testPortList[comIdx] = 0
		testPortListPassFail[comIdx] = "Pass"
	else:
	    if (cycle <= numSamplesFail[failCountList[comIdx]]):
		testPortList[comIdx] = 0
		testPortListPassFail[comIdx] = "Fail"
	    else:
		if (cycle >= numSamplesPass[failCountList[comIdx]]):
		    testPortList[comIdx] = 0
		    testPortListPassFail[comIdx] = "Pass"
	print "%s: %d/%d (number of misses/number of fixes)" % (myPort.comm.PortName, failCountList[comIdx], cycle)
	comIdx = comIdx + 1

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
	    print "3GPP test encounters error in geodeticNavigationDataHdlr handler function"  


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
    
    # num Samples Fail
    numSamplesFail = [2766,2766,2766,2766,2766,2766,42,52,64,75,87,100,112,125,139,152,166,180,194,208,222,237,251,266,281,295,310,325,341,356,371,387,402,418,433,449,465,480,496,512,528,544,560,576,592,608,625,641,657,674,690,706,723,739,756,772,789,805,822,839,855,872,889,906,922,939,956,973,990,1007,1024,1040,1057,1074,1091,1108,1126,1143,1160,1177,1194,1211,1228,1245,1263,1280,1297,1314,1331,1349,1336,1383,1401,1418,1435,1453,1470,1487,1505,1522,1540,1577,1574,1592,1609,1627,1644,1662,1679,1697,1714,1732,1750,1767,1785,1802,1820,1838,1855,1875,1890,1980,1926,1943,1961,1979,1997,2014,2032,2050,2067,2085,2103,2121,2139,2156,2174,2192,2210,22227,2245,2263,2281,2299,2317,2335,2352,2370,2388,2406,2424,2442,2460,2478,2496,2513,2531,2549,2567,2585,2603,2621,2639,2657,2675,2693,2711,2729,2747,2766]
    
    numSamplesPass = [77,106,131,154,176,197,218,238,257,277,295,314,333,351,369,387,405,422,440,457,474,492,509,526,543,560,577,593,610,627,643,660,676,693,709,725,742,758,774,790,807,823,839,855,871,887,903,919,935,951,967,982,998,1014,1030,1046,1061,1077,1108,1124,1140,1155,1171,1186,1202,1217,1233,1248,1264,1279,1295,1310,1326,1341,1357,1372,1387,1403,1418,1433,1449,1464,1479,1495,1510,1525,1540,1556,1571,1586,1601,1617,1632,1647,1662,1677,1692,1708,1723,1738,1753,1768,1783,1798,1813,1828,1844,1859,1874,1889,1904,1919,1934,1949,1964,1979,1994,2009,2024,2039,2054,2069,2084,2099,2114,2128,2143,2158,2173,2188,2203,2218,2233,2248,2263,2277,2292,2307,2322,2337,2352,2367,2381,2396,2411,2426,2441,2456,2470,2485,2500,2515,2530,2544,2559,2574,2589,2603,2618,2633,2648,2662,2677,2692,2702,2721,2736,2751,2765]
    
    # reference location
    AustraliaRefLocStr = "-37.8166667,144.966667,100"
    AtlantaRefLocStr = "33.75,-84.383333,300"
    refPositionList = [AtlantaRefLocStr,AustraliaRefLocStr]
    
    lat1 = 33 +  (45.0003170/60)
    lat2 = 37 + (48.9998/60)
    lon1 = 84 + (23.0001830/60)
    lon2 = 144 + (58.0002167/60)
    
    # test1To4RefLat = [33.75,-37.8166667]*8
    # test1To4RefLon = [-84.383333, 144.966667]*8
    test1To4RefLat = [lat1,-lat2]*8
    test1To4RefLon = [-lon1, lon2]*8
    test1To4RefAlt = [300,100]*8
    test1To4Geoid = [30.77,4.15]*8
    
    test1To4EphFiles = ["\\2005_Jan22_Day022.ai3eph","\\2004_Jan22_Day022.ai3eph"]*8
    test1To4AcqAssistFiles = ["\\AcquisitionAssist1.csv","\\AcquisitionAssist2.csv"]*8
    
    mainSimFilesDirs = "C:\\Program Files\\Spirent Communications\\SimPLEX\\Scenarios\\3GPP"
    test1To4SimFilesDirs = ["\\Test1-Sensitivity\\3GPP_Sensitivity_","\\Test2-NominalAccuracy\\3GPP_Nominal_","\\Test3-DynamicRange\\3GPP_Dynamic_","\\Test4-Multipath\\3GPP_Multipath_"]
    test1SimFiles = ["\\Spirent_3GPP_Sensitivity_Coarse_1.sim", "\\Spirent_3GPP_Sensitivity_Coarse_2.sim"]*8
    test2SimFiles = ["\\Spirent_3GPP_Normal_Accuracy_1.sim","\\Spirent_3GPP_Normal_Accuracy_2.sim"]*8
    test3SimFiles = ["\\Spirent_3GPP_Dynamic_1.sim", "\\Spirent_3GPP_Dynamic_2.sim"]*8
    test4SimFiles = ["\\Spirent_3GPP_Multipath_1.sim","\\Spirent_3GPP_Multipath_2.sim"]*8
    
    test1To4SimFiles = test1SimFiles + test2SimFiles + test3SimFiles + test4SimFiles
		     
    test5SimFile = mainSimFilesDirs + "\\Test5-MovingScenario\\SimPLEX_Spirent_3GPP_Moving_Scenario\\Spirent_3GPP_Moving_Scenario.sim"
    referenceSimFile = mainSimFilesDirs + "\\Reference\\SimPLEX_Spirent_3GPP_Normal_Accuracy_2\\Spirent_3GPP_Normal_Accuracy_2.sim"

    # 282,230
    simFilesWeekNum = [282,230]*8
    simFilesStartTOW = [345920,230720,346000,230800,346080,230880,346160,230960,346240,231040,346320,231120,346400,231200,346480,231280]
        
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
    
    strongestSignalLevel = [28,40,40,40,40]
    
    # get frequency transfer method
    freqTransferMethod = sCfg.get('AIDING_PARAMS','FREQ_TRANFER_METHOD')
    General.clsGlobal.LoopitTimeout = 61
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
	if (myPort.SignalViewLocation.IsOpen == False):
	    mainFrame.CreateSignalViewWin(myPort)
	if (myPort.DebugViewLocation.IsOpen == False):
	    mainFrame.CreateDebugViewWin(myPort)
	# myPort.comm.RxCtrl.OpenChannel("SSB")
	myPort.comm.RxCtrl.PollSWVersion()
	# myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)		
	# myPort.comm.RxCtrl.UTCOffset = 13
	failCountList.append(0)
	testPortListPassFail.append("Fail")	

	# intialize flags to indicate RX get TTFF
	scriptGlobals.getTTFFs.append(False)
	scriptGlobals.getNavs.append(False)	
	
	comIdx = comIdx + 1 
    
    testName = ["SENSITIVITY_TEST1", "NOMINAL_ACCURACY_TEST2", "DYNAMIC_RANGE_TEST3", "MULTIPATH_TEST4", "MOVING_SCENARIO_TEST5"]
    aidingName = ["MSB", "MSA1", "MSA2", "MSAB_B", "MSAB_A1", "MSAB_A2"]
    
    #Initialize
    print "Initializing ..."
    mainFrame.SimCtrl.EndScenario(False)
    mainFrame.Delay(5)
    
    scriptUtilities.init()
    mainFrame.Delay(10)
    mainFrame.SimCtrl.SelectScenario(referenceSimFile)
    
    comIdx = 0 
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue 
	myPort = scriptGlobals.DutPortManagersList[comIdx]   
	# if (rxFamily[comIdx] == "GSD4e"):
	# myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)		
	# myPort.comm.RxCtrl.UTCOffset = 13
	myPort.comm.RxCtrl.OpenChannel("SSB")
	myPort.comm.RxCtrl.OpenChannel("STAT")
	
	comIdx = comIdx+1
    logString = "Loading: " + referenceSimFile
    print logString
    mainFrame.SimCtrl.SetPowerLevel("-",scriptGlobals.CableLoss,1,True,True,False)
    mainFrame.SimCtrl.RunScenario()     
    
    # wait for nav
    waitTTFFLoops = 0
    while(waitTTFFLoops < 13):
	if(scriptUtilities.waitForNav() == True):
	    break
	else:
	    mainFrame.Delay(10)
	waitTTFFLoops = waitTTFFLoops + 1
    mainFrame.Delay(10)
    mainFrame.SimCtrl.EndScenario(False)
    mainFrame.Delay(5)
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
				
		currentTestRun = "%s %s Margin %s" % (aidingName[aidingTypeIndex],testName[test], marginStr)	
		General.clsGlobal.CurrentRunningTest = currentTestRun
		print "Current Test: %s" %(currentTestRun)
	
		comIdx = 0 
		for usePort in scriptGlobals.DutMainPortsList:
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue 
		    myPort = scriptGlobals.DutPortManagersList[comIdx]		        
		    myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)
		    myPort.comm.RxCtrl.UTCOffset = 13
		    myPort.comm.RxCtrl.PollSWVersion()
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
		    #myPort.comm.AutoReplyCtrl.FreqTransferCtrl.FreqOffset = General.clsGlobal.DEFAULT_RF_FREQ[rxDefaultClkIdx[comIdx]]		    
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
		
		    # Set debug level ACQ level 3
		    #myPort.comm.RxCtrl.SendRaw("A0A20033E400010101010101010101010101010101010501010101010101010101010101010101010101010101010101010101010101010119B0B3") 

		    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = True
		    #open log file
		    if (test < 4):			
			myPort.comm.RxCtrl.ResetCtrl.ResetCount = 0
			myPort.comm.Log.OpenFile(portLogFile);
			# myPort.comm.Log.SetDurationLoggingStatusLabel(portLogFile)
			myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)		    
		    
		    #mark to test port
		    testPortList[comIdx] = 1
		    failCountList[comIdx] = 0		    
		    testPortListPassFail[comIdx] = "N/A"
		    
		    comIdx = comIdx + 1
		
		# print "line 346"
			
		# test 5 no reset
		if (test < 4):		    
		    if (testCycles[test] <= 0):
			testSamples = 2766
		    else:
			testSamples = testCycles[test]
		    maxErrorAllow = getMaxErrorAllow(testSamples)
		    print "Max trials: %d -- Max Error Allow: %d" %(testSamples, maxErrorAllow)
		    for cycle in range(0,testSamples):
			if (scriptGlobals.TestAborted == True):
			    break
			testCycleString = "###### Session %d ######" % (cycle+1) 
			scriptUtilities.logApp("*",testCycleString)
			tmp = divmod(cycle,16)
			simFileIndex = tmp[1]
			simFilePath =  mainSimFilesDirs + test1To4SimFilesDirs[test] + scriptUtilities.format2Digits(simFileIndex+1) +  test1To4SimFiles[test*16 + simFileIndex] 
			logString = "Loading: " + simFilePath
			scriptUtilities.logApp("*", logString)
			mainFrame.SimCtrl.SelectScenario(simFilePath)
			mainFrame.Delay(2)
			# drop atten
			setSimPower(test, atten)
			scriptUtilities.logApp("*", "###### Turn off simulator power ######")
			mainFrame.SimCtrl.SetPowerOnOff("-", 1,False,True,True)
			mainFrame.Delay(1)
			mainFrame.SimCtrl.RunScenario()
			mainFrame.Delay(5)
			
			comIdx = 0 
			for usePort in scriptGlobals.DutMainPortsList:
			    if (usePort < 0):
				comIdx = comIdx + 1
				continue 
			    myPort = scriptGlobals.DutPortManagersList[comIdx]
			    		    
			    # set default
			    myPort.comm.RxCtrl.ResetCtrl.ResetInterval = General.clsGlobal.LoopitTimeout
			    myPort.comm.AutoReplyCtrl.AidingProtocolVersion = scriptGlobals.DutAi3ICDsList[comIdx]
			    # print "line 523"
			    localLat = "%.6f" % (test1To4RefLat[simFileIndex])
			    localLon = "%.6f" % (test1To4RefLon[simFileIndex])
			    localAlt = "%.2f" % (test1To4RefAlt[simFileIndex])
			    
			    # print localLat + " " + localLon + " " + localAlt
			    myPort.comm.RxCtrl.UpdateRefLocation(localLat, localLon, localAlt)
			    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Lat = test1To4RefLat[simFileIndex]
			    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Lon = test1To4RefLon[simFileIndex]
			    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Alt = test1To4Geoid[simFileIndex] + random.randint(0,500)
			    # myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Alt = random.randint(0,500)
			    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.DistanceSkew = random.randint(0,3000)
			    # myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.DistanceSkew = random.randint(0,2500)
			    myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.HeadingSkew = random.randint(0,360)
			    
			    print "Distance Skew = %d -- Heading Skew = %d" % (myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.DistanceSkew, myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.HeadingSkew)
			    
			    myPort.comm.AutoReplyCtrl.AutoReplyApproxPositionResp();
			    if ((aidingTypeIndex == 2) or (aidingTypeIndex == 5)): # MSA2 no position aiding
				myPort.comm.AutoReplyCtrl.ApproxPositionCtrl.Reject = True				
			    
			    myPort.comm.AutoReplyCtrl.TimeTransferCtrl.Skew = 0   
			    
			    myPort.comm.AutoReplyCtrl.EphFilePath = scriptAbsPath + test1To4EphFiles[simFileIndex]	
			    myPort.comm.AutoReplyCtrl.AcqDataFilePath =  scriptAbsPath + test1To4AcqAssistFiles[simFileIndex]	
			    print "Eph file: %s" % (myPort.comm.AutoReplyCtrl.EphFilePath)
			    myPort.comm.RxCtrl.RxNavData.ValidatePosition = True
			    
			    if (testPortList[comIdx] == 1):
				myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableEncryptedData = False;
				print "%s: Last clk drift = %.2f" % (myPort.comm.PortName, myPort.comm.LastClockDrift)
				print "%s: Last session clk drift = %.6f (%.6f)" % (myPort.comm.PortName,myPort.comm.LastAidingSessionReportedClockDrift * 1575.42,myPort.comm.LastAidingSessionReportedClockDrift)
			    
			    comIdx = comIdx + 1
			
			# add 10 seconds so the Sim can start correctly
			randStartTime = random.randint(1,60)
			
			logString = "Sleep %ds" % (randStartTime)
			scriptUtilities.logApp("*",logString)
			mainFrame.Delay(randStartTime)			
			
			scriptUtilities.logApp("*", "###### Turn on simulator power ######")
			mainFrame.SimCtrl.SetPowerOnOff("-", 1,True,True,True)
			if (test == 3):
			    # turn off chans 9,10,11,12
			    mainFrame.SimCtrl.SetPowerOnOff("-", 8,False,True,False)
			    mainFrame.SimCtrl.SetPowerOnOff("-", 9,False,True,False)
			    mainFrame.SimCtrl.SetPowerOnOff("-", 10,False,True,False)
			    mainFrame.SimCtrl.SetPowerOnOff("-", 11,False,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 0,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 1,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 2,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 3,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 4,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 5,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 6,True,True,False)
			# mainFrame.SimCtrl.SetPowerOnOff("-", 7,True,True,False)
			secIntoRun = mainFrame.SimCtrl.GetTimeIntoRun()
			simTowNow = (simFilesStartTOW[simFileIndex]*1.5 + int(secIntoRun)) * 100
			# do this quick so start it separately
			comIdx = 0 
			for usePort in scriptGlobals.DutMainPortsList:
			    if (usePort < 0):
				comIdx = comIdx + 1
				continue 
			    myPort = scriptGlobals.DutPortManagersList[comIdx]
			    myPort.comm.RxCtrl.SetGPSTime(`simFilesWeekNum[simFileIndex]`,`simTowNow`)
			    
			    comIdx = comIdx + 1	
			
			print "%s: SIM GPS TOW = %.6f" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),simTowNow)
			mainFrame.Delay(10)
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
			
			mainFrame.SimCtrl.EndScenario(False)
			mainFrame.Delay(10)
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
			checkResults(cycle+1)
			if ((checkTestProgress() == False) or (scriptGlobals.TestAborted == True)):
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
			
			# myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = False			
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = True
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_Eph_fromFile = False
			myPort.comm.AutoReplyCtrl.AutoReplyParams.AutoAid_AcqData_fromFile = False
			myPort.comm.AutoReplyCtrl.EphFilePath = scriptAbsPath + test1To4EphFiles[1]			
			
			myPort.comm.AutoReplyCtrl.FreqTransferCtrl.DefaultFreqIndex = scriptGlobals.DutDefaultClkIndicesList[comIdx]
			#myPort.comm.AutoReplyCtrl.FreqTransferCtrl.FreqOffset = General.clsGlobal.DEFAULT_RF_FREQ[rxDefaultClkIdx[comIdx]]		    
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
			# myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)
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
    myReport.Percentile = "95"
    myReport.TTFFLimit = "20"
    myReport.HrErrLimit = "100"
    myReport.LimitVal = "20"
    myReport.TTFFReportType = 3
    myReport.TimeoutVal = `General.clsGlobal.LoopitTimeout`
    myReport.Summary_3GPP(logDirectory)
    #cleanup
    comIdx = 0
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue 
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
	comIdx = comIdx + 1
    print "3GPP test completed" 

    mainFrame.UpdateGUIFromScript()
except:
    scriptUtilities.ExceptionHandler()


    
    
