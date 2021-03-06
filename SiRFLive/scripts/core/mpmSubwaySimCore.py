import scriptGlobals
import scriptUtilities
import scriptSim

def rxInit():
    scriptUtilities.openPorts()
	
    # set atten
    scriptUtilities.setAtten(defaultAtten)

    # setup each active com ports     
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	myPort.comm.RxCtrl.SaveTestSetup("RX COM Port","Com" + `usePort`)
	myPort.comm.RxCtrl.SaveTestSetup("RX Serial Number",scriptGlobals.DutNamesList[comIdx])
		
	myPort.comm.RxCtrl.PollSWVersion()
	myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)	
	myPort.comm.RxCtrl.SetValidatePostionFlag(True)
	myPort.comm.RxCtrl.OpenChannel("SSB")
	myPort.comm.RxCtrl.OpenChannel("STAT") 
	
	comIdx = comIdx + 1    
    # Send cold so SGEE would work
    scriptUtilities.init("COLD")
    
try:
    General.clsGlobal.Abort = False
    General.clsGlobal.AbortSingle = False
    scriptGlobals.Exiting = False
    scriptGlobals.TestAborted = False
    
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()    
    sCfg.read(scriptGlobals.ScriptConfigFilePath)
    
    # timeout = 26
    General.clsGlobal.LoopitTimeout = int(scriptUtilities.ConvertToDouble(sCfg.get('TEST_PARAM','TIMEOUT')))
    General.clsGlobal.LoopitIteration = sCfg.getint('TEST_PARAM','NUMBER_OF_LOOP') 
    General.clsGlobal.LoopitResetType = sCfg.get('TEST_PARAM','RESET_TYPE')
    
    #scriptGlobals.MainFrame.AutoTestAbortHdlr.SetSiRFLiveEventHdlr += scriptUtilities.abortTest 
    testContinue = True
    if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
	if (scriptSim.isSimRunning() == True):
	    result = MessageBoxEx.Show("SIM is running -- Proceed?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000)
	    if (result == DialogResult.Yes):
		testContinue = True
		scriptSim.simStop()
		scriptSim.simLoad(scriptGlobals.SimFile)
		# scriptSim.simSetAllChanAtten(scriptGlobals.SimInitialAtten)
		scriptSim.simRun()
	    else:
		testContinue = False
	else:
	    scriptSim.simLoad(scriptGlobals.SimFile)
	    # scriptSim.simSetAllChanAtten(scriptGlobals.SimInitialAtten)
	    scriptSim.simRun()
    scriptGlobals.MainFrame.Delay(5)
    if (testContinue == True):	
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
	scriptGlobals.getTTFFs = []
	scriptGlobals.getNavs = []

	rxInit()
		
	# wait for nav
	waitTTFFLoops = 0
	while(waitTTFFLoops < 13):
	    if(scriptUtilities.waitForNav() == True):
		break
	    else:
		mainFrame.Delay(10)
	    waitTTFFLoops = waitTTFFLoops + 1
	    mainFrame.Delay(10)	
	
	# Begin test
	scriptUtilities.logApp("*", scriptGlobals.TestBeginLabel)
	print "Number of test levels %d" % (len(scriptGlobals.TestSignalLevelsList))
	for levelIndex in range(0, len(scriptGlobals.TestSignalLevelsList)):
	    level = scriptGlobals.TestSignalLevelsList[levelIndex]    
	    atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
	    if (scriptGlobals.TestAborted == True):
		break
	    scriptUtilities.logApp("*", scriptGlobals.InitializedLabel)
	    # Create directory for log files
	    Now = time.localtime(time.time())
	    timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		myPort.comm.RxCtrl.PollSWVersion()
		baseName = "%s%s_%s_%s_%s_%s" %(scriptGlobals.TestResultsDirectory,timeNowStr,scriptGlobals.DutNamesList[comIdx],myPort.comm.PortName,scriptGlobals.ScriptName,level)		
		portLogFile = baseName + scriptGlobals.LogFileExtsList[comIdx]
		ttffLogFile = baseName + "_ttff.csv"
		# update Test Info	    
		if (level < 0):
		    testSetupLevelString = "%s%s%s" %("m",`-level`.replace('.','p'),scriptGlobals.SignalType) 	
		else:
		    testSetupLevelString = "%s%s" %(`level`.replace('.','p'),scriptGlobals.SignalType) 
		    
		scriptGlobals.TestID = "%s-%s" % (scriptGlobals.TestName,testSetupLevelString)	 
		Now = time.localtime(time.time())
		scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
		myPort.comm.RxCtrl.DutStationSetup.SignalLevel = level
		scriptUtilities.updateDUTInfo(comIdx)
		myPort.comm.m_TestSetup.Atten = atten
		
		myPort.comm.Log.OpenFile(portLogFile)		
		myPort.comm.RxCtrl.ResetCtrl.OpenTTFFLog(ttffLogFile)
		myPort.comm.RxCtrl.ResetCtrl.ResetCount = 0
		myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = True
		myPort.comm.RxCtrl.ResetCtrl.ResetInterval = General.clsGlobal.LoopitTimeout
		# if not CGEE test disable CGEE since CGEE is enable by default
		if (mpmTestCondition.Contains("CGEE") == False):
		    # myPort.comm.WriteData("A0 A2 00 06 E8 FE 00 00 00 00 01 E6 B0 B3")
		    myPort.comm.WriteData("A0 A2 00 04 E8 20 00 01 01 09 B0 B3");
		comIdx = comIdx + 1
	    scriptUtilities.setResetInitParams()
	    
	    logStr = "#### Dropping atten to desired level %f ####" % (level)
	    scriptUtilities.logApp("*", logStr)
	    
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
	    
	    navStatusCntArray = len(scriptGlobals.DutMainPortsList) * [0]
	    
	    for l1 in range(0,General.clsGlobal.LoopitIteration):
		if (scriptGlobals.TestAborted == True):
		    break
		atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
		scriptUtilities.setAtten(atten)	
		# Send MPM
		if (General.clsGlobal.LoopitIteration > 1):
		    logStr = "####### MPM Loop Number %d of %d ######" % (l1+1, General.clsGlobal.LoopitIteration)
		    scriptUtilities.logApp("*",logStr)		
		if (mpmSoakTimeSecs != 0):
		    displayStr = "%s: Wait %d sec for DUT to collect GPS information%s Press OK to cancel wait time" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),mpmSoakTimeSecs, "\r\n\r\n    ")
		    scriptUtilities.logApp("*",displayStr)
		    result = MessageBoxEx.Show(displayStr, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,int(mpmSoakTimeSecs)*1000);        
		comIdx = 0    
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]	    
		    myPort.comm.RxCtrl.SendMPM_V2(mpmTimeout, mpmControlMode)
		    comIdx = comIdx + 1
		scriptUtilities.logApp("*", "### Wait 30s ###")
		mainFrame.Delay(30)
		# turn off sim signal
		scriptSim.simOff()
		if (mpmCollectionTimeSecs != 0):
		    displayStr = "%s: Wait %d sec for DUT to collect MPM information%s Press OK to cancel wait time" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),mpmCollectionTimeSecs, "\r\n\r\n    ")
		    scriptUtilities.logApp("*",displayStr)
		    result = MessageBoxEx.Show(displayStr, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,int(mpmCollectionTimeSecs)*1000);
		atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level-10,scriptGlobals.SignalType)
		scriptUtilities.setAtten(atten)		
		# scriptSim.simSetAllChanAtten(-10)
		for ic in range(0,6):
		    scriptSim.simOnChan(ic)
		    mainFrame.Delay(10)
		# bring DUT out of MPM
		scriptUtilities.logApp("*","#### Command exit MPM ####") 
		comIdx = 0    
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]	
		    myPort.comm.LowPowerParams.Mode = 0	
		    myPort.comm.RxCtrl.ResetCtrl.ResetCount = myPort.comm.RxCtrl.ResetCtrl.ResetCount + 1
		    myPort.comm.RxCtrl.ResetCtrl.DisplayResetType = "MPM";
		    myPort.comm.RxCtrl.ResetCtrl.LogThisReset = True
		    myPort.comm.RxCtrl.ResetCtrl.ResetTTFFAvailable = False 
		    myPort.comm.RxCtrl.ResetCtrl.ResetPositionAvailable = False	
		    myPort.comm.RxCtrl.SetPowerMode(False)
		    myPort.comm.Toggle4eWakeupPort()
		    comIdx = comIdx + 1
		 
		navStatusArray = len(scriptGlobals.DutMainPortsList) * [False]
		count = 0
		startDelay = General.clsGlobal.LoopitTimeout/10;
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
				logStr = "Loop %d: %s Rx nav -- %d/%d loops" % (l1+1, myPort.comm.PortName,navStatusCntArray[comIdx],General.clsGlobal.LoopitIteration)
				myPort.comm.WriteApp(logStr);
				print logStr		    
			comIdx = comIdx + 1
		    
		    navStatus = True
		    for tmpStatus in navStatusArray:
			navStatus =  navStatus and tmpStatus
		    count = count + 1
		    mainFrame.Delay(10)
		    if (navStatus == True):	
			# all RXs nav
			break		

		#if (navStatus == False):		
		comIdx = 0	
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    myPort.comm.RxCtrl.ResetCtrl.ResetTTFFAvailable = False
		    myPort.comm.RxCtrl.ResetCtrl.LogTTFFCsv()    
		    myPort.comm.RxCtrl.ResetCtrl.ResetPositionAvailable = False	
		    		
		    comIdx = comIdx + 1
		    
		# stop/rewind sim and run sim
		scriptSim.simStop()
		mainFrame.Delay(20)
		scriptSim.simRewind()
		scriptSim.simRun()
		scriptUtilities.logApp("*", "Reset since Sim restarts")
		scriptUtilities.reset("COLD")
	    
	    comIdx = 0    
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		myPort.comm.LowPowerParams.Mode = 0		    
		myPort.comm.RxCtrl.SetPowerMode(False)
		myPort.comm.Toggle4eWakeupPort()
		comIdx = comIdx + 1
	    
	    almCollectionTime = 300
	    displayStr = "%s: Wait %d sec for DUT to collect ALM information%s Press OK to cancel wait time" % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),almCollectionTime, "\r\n\r\n    ")
	    scriptUtilities.logApp("*",displayStr)
	    result = MessageBoxEx.Show(displayStr, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,int(almCollectionTime)*1000);
	    
	    comIdx = 0    
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		myPort.comm.Log.CloseFile();
		myPort.comm.RxCtrl.ResetCtrl.CloseTTFFLog()
		comIdx = comIdx + 1
	
	# Done now clean up
	scriptGlobals.Exiting = True
	comIdx = 0    
	for usePort in portList:
	    if (usePort < 0):
		comIdx = comIdx + 1
		continue
	    myPort = scriptGlobals.DutPortManagersList[comIdx]
	    myPort.comm.RxCtrl.ResetCtrl.LoopitInprogress = False
	    myPort.comm.ClosePort()
	    # myPort.StopAsyncProcess()
	    comIdx = comIdx + 1	
    
    scriptGlobals.MainFrame.SetScriptDone(True)
    mainFrame.UpdateGUIFromScript()
    
except:
    scriptUtilities.ExceptionHandler()