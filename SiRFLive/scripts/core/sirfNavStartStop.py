clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import *
import time
import random

import scriptGlobals
import scriptUtilities 
import scriptSim

try:   
    General.clsGlobal.Abort = False
    General.clsGlobal.AbortSingle = False
    scriptGlobals.Exiting = False
    scriptGlobals.TestAborted = False
    scriptGlobals.UseTTBForAiding=0
    
    # #scriptGlobals.MainFrame.AutoTestAbortHdlr.SetSiRFLiveEventHdlr += scriptUtilities.abortTest  

    failCountList = []
    testPortListPassFail = []    

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
    
    # timeout = 26
    timeout = sCfg.getint('TEST_PARAM','TIMEOUT')
    Loop = sCfg.getint('TEST_PARAM','NUMBER_OF_LOOP') 
    stopDelay = sCfg.getint('TEST_PARAM','OFF_TIME')  
    
    testLevelStr = sCfg.get('TEST_PARAM','SIGNAL_LEVEL')
    scriptGlobals.SignalType = sCfg.get('TEST_PARAM','SIGNAL_TYPE')
    scriptGlobals.CableLoss = sCfg.getfloat('TEST_PARAM','CABLE_LOSS') 
    nbWaitTime = sCfg.getint('TEST_PARAM','SOAK_TIME_SEC')
    collectionTime = sCfg.getint('TEST_PARAM','COLLECTION_TIME_SEC')
    
    scriptGlobals.SimFile = sCfg.get('SIM','SIM_FILE')
        
    scriptGlobals.TestSignalLevelsList = Utilities.HelperFunctions.ParseTestLevels(testLevelStr) 
    
    # read config SiRFNav params
    tmpV = sCfg.get('SIRFNAV_PARAM','VERSION')
    scriptGlobals.SiRFNavStartStop_Version = eval('['+tmpV+']')
    scriptGlobals.SiRFNavStartStop_StartMode = sCfg.getint('SIRFNAV_PARAM','START_MODE')
    scriptGlobals.SiRFNavStartStop_UARTMaxPreamble = sCfg.getint('SIRFNAV_PARAM','UART_MAX_PREAMBLE')
    scriptGlobals.SiRFNavStartStop_UARTIdleByteWakeupDelay = sCfg.getint('SIRFNAV_PARAM','UART_IDLE_BYTE_WAKEUP_DELAY')
    #scriptGlobals.SiRFNavStartStop_ReferenceClockOffset = int(rxDefaultClk[comIdx])#0x177FA
    scriptGlobals.SiRFNavStartStop_FlowControl = sCfg.getint('SIRFNAV_PARAM','FLOW_CONTROL')
    #scriptGlobals.SiRFNavStartStop_LNAType = int(rxLNAType[comIdx])
    scriptGlobals.SiRFNavStartStop_DebugSettings = sCfg.getint('SIRFNAV_PARAM','DEBUG_SETTINGS')
    scriptGlobals.SiRFNavStartStop_ReferenceClockWarmupDelay = sCfg.getint('SIRFNAV_PARAM','REFERENCE_CLOCK_WARMUP_DELAY')
    # myPort.comm.SiRFNavStartStop.ReferenceClockFrequency =  0xf9c568 # 0x18CBA80 
    #scriptGlobals.SiRFNavStartStop_ReferenceClockFrequency =  int(rxExtClkFreq[comIdx]) 
    scriptGlobals.SiRFNavStartStop_ReferenceClockUncertainty = sCfg.getint('SIRFNAV_PARAM','REFERENCE _CLOCK_UNCERTAINTY')
    scriptGlobals.SiRFNavStartStop_BaudRate = sCfg.getint('SIRFNAV_PARAM','BAUD_RATE')
    scriptGlobals.SiRFNavStartStop_IOPinConfigurationMode = sCfg.getint('SIRFNAV_PARAM','IOPIN_CONFIG_MODE')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration0 = sCfg.getint('SIRFNAV_PARAM','IO_PIN0')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration1 = sCfg.getint('SIRFNAV_PARAM','IO_PIN1')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration2 = sCfg.getint('SIRFNAV_PARAM','IO_PIN2')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration3 = sCfg.getint('SIRFNAV_PARAM','IO_PIN3')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration4 = sCfg.getint('SIRFNAV_PARAM','IO_PIN4')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration5 = sCfg.getint('SIRFNAV_PARAM','IO_PIN5')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration6 = sCfg.getint('SIRFNAV_PARAM','IO_PIN6')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration7 = sCfg.getint('SIRFNAV_PARAM','IO_PIN7')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration8 = sCfg.getint('SIRFNAV_PARAM','IO_PIN8')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration9 = sCfg.getint('SIRFNAV_PARAM','IO_PIN9')
    scriptGlobals.SiRFNavStartStop_IOPinConfiguration10 = sCfg.getint('SIRFNAV_PARAM','IO_PIN10')
    scriptGlobals.SiRFNavStartStop_I2CHostAddress = sCfg.getint('SIRFNAV_PARAM','I2C_HOST_ADDR')
    scriptGlobals.SiRFNavStartStop_I2CTrackerAddress = sCfg.getint('SIRFNAV_PARAM','I2C_TRACKER_ADDR')
    scriptGlobals.SiRFNavStartStop_I2CMode = sCfg.getint('SIRFNAV_PARAM','I2C_MODE')
    scriptGlobals.SiRFNavStartStop_I2CRate = sCfg.getint('SIRFNAV_PARAM','I2C_RATE')
    scriptGlobals.SiRFNavStartStop_SPIRate = sCfg.getint('SIRFNAV_PARAM','SPI_RATE')
    scriptGlobals.SiRFNavStartStop_ONOffControl = sCfg.getint('SIRFNAV_PARAM','ON_OFF_CONTROL')
    scriptGlobals.SiRFNavStartStop_FlashMode = sCfg.getint('SIRFNAV_PARAM','FLASH_MODE')
    scriptGlobals.SiRFNavStartStop_StorageMode = sCfg.getint('SIRFNAV_PARAM','STORAGE_MODE')
    scriptGlobals.SiRFNavStartStop_TrackerPort = sCfg.get('SIRFNAV_PARAM','TRACKER_PORT') # for Phytec put "C"
    scriptGlobals.SiRFNavStartStop_TrackerPortSeleted = sCfg.getint('SIRFNAV_PARAM','TRACKER_PORT_SELECT')
    scriptGlobals.SiRFNavStartStop_WeakSignalEnable = sCfg.getint('SIRFNAV_PARAM','WEAK_SIG_ENABLE')
    # scriptGlobals.SiRFNavStartStop_LDOEnable = int(rxLDOMode[comIdx])   
    
    # scriptGlobals.TestSignalLevelsList = Utilities.HelperFunctions.ParseTestLevels(testLevelStr)   
    
    if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
	if (scriptSim.isSimRunning() == True):
	    result = MessageBoxEx.Show("SIM is running -- Proceed?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000)
	    if (result == DialogResult.Yes):
		testContinue = True
		scriptSim.simStop()
		scriptSim.simLoad(scriptGlobals.SimFile)		
		scriptSim.simRun()
	    else:
		testContinue = False
	else:
	    scriptSim.simLoad(scriptGlobals.SimFile)	    
	    scriptSim.simRun()
    scriptGlobals.MainFrame.Delay(5)
    # set to high level for factory reset
    if (scriptGlobals.SignalType.ToLower() == "dbhz"):
	defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,40,scriptGlobals.SignalType)
    elif (scriptGlobals.SignalType.ToLower() == "dbm"):
	defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,-130,scriptGlobals.SignalType)
    else:
	print "Signal Type is not correct"
	defaultAtten = 0
    
    # set atten 
    scriptUtilities.setAtten(defaultAtten)    
    
    testPowerStr = ""
    for testPower in scriptGlobals.TestSignalLevelsList:
	testPowerStr = testPowerStr + " " + `testPower`
    print "Test power levels: %s" % (testPowerStr)

    # setup each active com ports    
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# reset logfile name
	myPort.comm.Log.SetDurationLoggingStatusLabel("")
	myPort.comm.OpenPort()
	myPort.RunAsyncProcess()
	myPort.comm.RxCtrl.PollSWVersion()
	
	if (myPort.SignalViewLocation.IsOpen == False):
	    mainFrame.CreateSignalViewWin(myPort)
	if (myPort.DebugViewLocation.IsOpen == False):
	    mainFrame.CreateDebugViewWin(myPort)
	
	comIdx = comIdx + 1
    
    # Config SiRFNavStart
    scriptUtilities.updateSiRFNavStartParams()
    mainFrame.UpdateGUIFromScript()
    print "Test levels %d" % (len(scriptGlobals.TestSignalLevelsList))
    for levelIndex in range(0, len(scriptGlobals.TestSignalLevelsList)):
	if (scriptGlobals.TestAborted == True):
	    break
	level = scriptGlobals.TestSignalLevelsList[levelIndex]
	scriptUtilities.setAtten(defaultAtten) 
	atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
	# nbWaitTime = 900
	msgString = "Wait %d seconds to collect GPS information?" % (nbWaitTime)
	if (scriptGlobals.UseTTBForAiding == 0):
	    # last element is timeout in ms
	    result = MessageBoxEx.Show(msgString, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000);
	    if (result == DialogResult.Yes):
		if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
		    scriptGlobals.MainFrame.SimCtrl.EndScenario(False)
		    scriptGlobals.MainFrame.SimCtrl.SelectScenario(scriptGlobals.SimFile)
		    scriptGlobals.MainFrame.SimCtrl.SetPowerLevel("-",scriptGlobals.SimInitialAtten,1,True,True,True)
		    scriptGlobals.MainFrame.SimCtrl.RunScenario() 

		    scriptUtilities.sendFactoryReset()		    
		    
		    # ttbWaitTime = 2
		displayStr = "%s: Wait %d sec for RX to navigate and collectGPS information %s Press OK to cancel wait time" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),nbWaitTime, "\r\n\r\n    ")
		scriptUtilities.logApp("*",displayStr)
		result = MessageBoxEx.Show(displayStr, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,nbWaitTime*1000);	
	
	scriptUtilities.updateSiRFNavStartParams()
	if (scriptGlobals.TestAborted == True):
	    print "Test aborted"
	    break		
	print "Testing power level: %d" % (level)
	# setup each active com ports  
	# Create directory for log files
	Now = time.localtime(time.time())
	timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
	
	comIdx = 0	
	for usePort in scriptGlobals.DutMainPortsList:	
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    myPort.comm.RxCtrl.SaveTestSetup("RX COM Port","Com" + `usePort`)
	    myPort.comm.RxCtrl.SaveTestSetup("RX Serial Number",scriptGlobals.DutNamesList[comIdx])
	    baseName = "%s%s%s_%s_%s_%s_%s_%ss" %(scriptGlobals.TestResultsDirectory,timeNowStr,scriptGlobals.DutNamesList[comIdx],myPort.comm.PortName,scriptGlobals.ScriptName,`level`,`stopDelay`,`stopDelay`)
	    portLogFile = baseName + scriptGlobals.LogFileExtsList[comIdx]
	    # ttffLogFile = baseName + "_ttff.csv"
	    	    
	    # update Test Info	    
	    if (level < 0):
		testSetupLevelString = "%s%s%s" %("m",`-level`.replace('.','p'),scriptGlobals.SignalType) 	
	    else:
		testSetupLevelString = "%s%s" %(`level`.replace('.','p'),scriptGlobals.SignalType) 
		
	    scriptGlobals.TestID = "%s-%ds-%s" % (scriptGlobals.TestName,stopDelay,testSetupLevelString)	 
	    Now = time.localtime(time.time())
	    scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
	    myPort.comm.RxCtrl.DutStationSetup.SignalLevel = level
	    scriptUtilities.updateDUTInfo(comIdx)
	    myPort.comm.m_TestSetup.Atten = atten
	    
	    myPort.comm.Log.OpenFile(portLogFile)	
	    # myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)
	    myPort.comm.RxCtrl.PollSWVersion()
	    myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)	
	    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = True
	    myPort.comm.RxCtrl.ResetCtrl.ResetInterval = timeout
	    
	    # myPort.UpdateGUIFromComm()
	    
	    myPort.comm.RxCtrl.SetValidatePostionFlag(True)
	    myPort.comm.RxCtrl.OpenChannel("SSB")
	    myPort.comm.RxCtrl.OpenChannel("STAT") 
	    
	    comIdx = comIdx + 1
	
	scriptUtilities.sendFactoryReset()
	scriptUtilities.updateSiRFNavStartParams()
	mainFrame.Delay(5)
	navStatus = False
	count = 0
	while(count < 60):
	    navStatus = mainFrame.GetNavStatus("*")
	    count = count + 1
	    print "Wait for nav loop %d" %(count)
	    if ((navStatus == True) or (count >= 60)): 		
		break
	    # if ((General.clsGlobal.Abort == True) or (General.clsGlobal.AbortSingle == True)):
		# testAborted = True
		# break
	    mainFrame.Delay(10)
	
	# if (testAborted == True):
	    # print "Test aborted"
	    # sys.exit(0)
	
	ephCollectTime = 30
	if(navStatus == True):
	    print "RX(s) navigate"
	else:
	    print "Rx(s) not navigate"
	print " -- Test will start after %d seconds collecting GPS information ..." % (ephCollectTime)
	mainFrame.Delay(ephCollectTime)
	
	comIdx = 0	
	for usePort in scriptGlobals.DutMainPortsList:	
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    # open TTFF here to ignore first commanded reset
	    baseName = "%s%s%s_%s_%s_%s_%s_%ss" %(scriptGlobals.TestResultsDirectory,timeNowStr,scriptGlobals.DutNamesList[comIdx],myPort.comm.PortName,scriptGlobals.ScriptName,`level`,`stopDelay`,`stopDelay`)	    
	    ttffLogFile = baseName + "_ttff.csv"
	    myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)
	    myPort.comm.RxCtrl.ResetCtrl.ResetCount = 0
	    comIdx = comIdx + 1	    
	
	diffAtten = atten - defaultAtten
	if ((levelIndex == 0) and (diffAtten > 5) and (atten > 5)):
	    dropAtten = divmod(atten,5)
	    drop5dBLoop = dropAtten[0]
	    restAtten = dropAtten[1]
	    
	    for dropIndex in range(0, int(drop5dBLoop)+1):		
		atten1 = 5*dropIndex + defaultAtten
		scriptUtilities.setAtten(atten1)		
		mainFrame.Delay(20)
	    
	    atten1 = restAtten +  atten1
	    scriptUtilities.setAtten(atten1)
	    
	else:
	    scriptUtilities.setAtten(atten)	
            
        startTime = time.clock()
	navStatusCntArray = len(scriptGlobals.DutMainPortsList) * [0]
	scriptUtilities.updateResetType()
	resetIdx = 0
	resetTotal = 1
	startTestTime = time.clock()
	if (General.clsGlobal.LoopitIteration < 0):
	    displayStr = "%s: Reset Test runs for %ds" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),collectionTime)
	    resetTotal = 1
	    print displayStr
	else:
	    resetTotal = General.clsGlobal.LoopitIteration
	# for resetIdx in range(0,General.clsGlobal.LoopitIteration):
	while (resetIdx < resetTotal):
	    if (scriptGlobals.TestAborted == True):
		break
	    stopTime = time.clock()
	    elapsedTestTime = stopTime - startTestTime
	    if ((General.clsGlobal.LoopitIteration < 0) and
		(collectionTime > 0) and (elapsedTestTime > collectionTime)):
		# stop test now		    
		break
	# for resetIdx in range(0,Loop):
	    
	    # Send sirfNavStart
	    logStr = "Loop %d of %d" % (resetIdx+1, Loop)
	    scriptUtilities.logApp("*",logStr)
	    
	    logStr = "Send SirfNavStop"
	    scriptUtilities.logApp("*",logStr)
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		# myPort.comm.RxCtrl.SendRaw("A0A2 0002 A1 08 00A9 B0B3")
		myPort.comm.RxCtrl.SiRFNavStop();
		comIdx = comIdx + 1
	    
	    logStr =  "%s: Wait %d" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),stopDelay)
	    # logStr =  "Sleep %d" %(stopDelay)
	    scriptUtilities.logApp("*",logStr)
	    mainFrame.Delay(stopDelay)
	    logStr = "Send SirfNavStart"
	    scriptUtilities.logApp("*",logStr)
	    comIdx = 0
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		# myPort.comm.RxCtrl.SendRaw("A0A2 0002 A1 06 00A7 B0B3")
		# myPort.comm.RxCtrl.ResetCtrl.ResetCount = resetIdx+1
		# myPort.comm.RxCtrl.ResetCtrl.LogThisReset = True
		# myPort.comm.m_NavData.IsNav = False
		# myPort.comm.MonitorNav = True
		myPort.comm.SendFlag = 2
		myPort.comm.RxCtrl.SiRFNavStart();
		comIdx = comIdx + 1
	    
	    # mainFrame.Delay(20)
	    startDelay = timeout/10;
	    
	    navStatusArray = len(scriptGlobals.DutMainPortsList) * [False]
	    count = 0
	    while(count <= startDelay):
		print "Wait for nav loop %d" %(count)
		comIdx = 0    
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    if (navStatusArray[comIdx] == False):
		    	navStatusArray[comIdx] = mainFrame.GetNavStatus(myPort.comm.PortName)		    
	    
		        if (navStatusArray[comIdx] == True):
			    navStatusCntArray[comIdx] = navStatusCntArray[comIdx] + 1			
			    logStr = "Loop %d: %s Rx nav -- %d/%d loops" % (resetIdx+1, myPort.comm.PortName,navStatusCntArray[comIdx],Loop)
			    myPort.comm.WriteApp(logStr);
			    print logStr		    
		    comIdx = comIdx + 1
		
		navStatus = True
		for tmpStatus in navStatusArray:
		    navStatus =  navStatus and tmpStatus
		count = count + 1		
		if ((navStatus == True) and (earlyCompletionFlag==1)):	
		    # all RXs nav
		    break		
		mainFrame.Delay(10)
	    
	    if (navStatus == False):		
		comIdx = 0	
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    if (navStatusArray[comIdx] == False):
			myPort.comm.RxCtrl.ResetCtrl.ResetTTFFAvailable = False
			myPort.comm.RxCtrl.ResetCtrl.ResetPositionAvailable = False			
			myPort.comm.RxCtrl.ResetCtrl.LogTTFFCsv()		
		    comIdx = comIdx + 1
	    
	    stopTime = time.clock()
	    waitEph = False
	    # Autonomous mode
	    if (scriptGlobals.IsAidingTest == 0):
		#if (signalType.ToLower() == "dbhz"):
		#    if (level < 28):
		#	waitEph = True
			
		#if (signalType.ToLower() == "dbm"):
		#    if (level < -142):
		#	waitEph = True
		waitEph = True
		elapsedTime = stopTime - startTime    
		if ((waitEph == True) and (elapsedTime > 3600)):
		    # General.clsGlobal.WaitForEph = True
		    # Utilities.HelperFunctions.ResetTimeStop(False)
		    ephWaitTime = 900
		    attenStr = "###### Setting Attenuation to " + `defaultAtten` + "dB and wait" + `ephWaitTime` + "seconds for nav #######"
		    scriptUtilities.logApp("*", attenStr)
		    # set atten 
		    scriptUtilities.setAtten(defaultAtten)
		    mainFrame.Delay(ephWaitTime)
		    if (scriptGlobals.TestAborted == False):
			# General.clsGlobal.WaitForEph = False
			scriptUtilities.setAtten(atten)
			startTime = time.clock()
			# Utilities.HelperFunctions.ResetTimeStart(False)
	    # mainFrame.Delay(startDelay)	    
	    # print "Sleep %d" %(startDelay)
	    resetIdx = resetIdx + 1
	    if (collectionTime > 0):
		resetTotal = resetTotal + 1
	comIdx = 0    
	for usePort in scriptGlobals.DutMainPortsList:	
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    #myPort.comm.ListenersCtrl.Stop(General.clsGlobal.CNO_LABEL, myPort.comm.PortName)
	    #myPort.comm.ListenersCtrl.Stop(General.clsGlobal.TTFF_LABEL, myPort.comm.PortName)
	    #myPort.comm.ListenersCtrl.Stop(General.clsGlobal.TTFF_MSA_LABEL, myPort.comm.PortName)
	    #myPort.comm.ListenersCtrl.Stop(General.clsGlobal.POSITION_RESPONSE_LABEL, myPort.comm.PortName)
	    #myPort.comm.ListenersCtrl.Stop(General.clsGlobal.MEASUREMENT_RESPONSE_LABEL, myPort.comm.PortName)	
	    #myPort.comm.ListenersCtrl.Stop(General.clsGlobal.GEODETIC_NAVIGATION_DATA_LABEL, myPort.comm.PortName)
	    myPort.comm.Log.CloseFile();
	    myPort.comm.RxCtrl.ResetCtrl.CloseTTFFLog()	    
	    comIdx = comIdx + 1
	
    # Done now clean up
    scriptGlobals.Exiting = True
    comIdx = 0    
    for usePort in scriptGlobals.DutMainPortsList:
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.ClosePort()
	myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
	# myPort.StopAsyncProcess()
	comIdx = comIdx + 1
    
    timeoutSpec = timeout - 1;
    ttffLimitStr = `timeoutSpec` + "," + `timeoutSpec` + "," + `timeoutSpec`

    myReport = mainFrame.ReportCtrl
    myReport.Percentile = "50,67,95"
    myReport.TTFFLimit = ttffLimitStr
    myReport.HrErrLimit = "50,100,150"
    myReport.TimeoutVal = `timeoutSpec`
    myReport.LimitVal = "100"
    myReport.TTFFReportType = 3;
    myReport.Summary_Reset(logDirectory)	

    mainFrame.SetScriptDone(True)
    mainFrame.UpdateGUIFromScript()
    print "Done: %s" % (scriptGlobals.ScriptName)
	
except:   
    scriptUtilities.ExceptionHandler()
    
    
