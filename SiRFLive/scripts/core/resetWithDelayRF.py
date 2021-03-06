import scriptGlobals
import scriptUtilities
import scriptSim

try:  
    # Read configuration parameters
    # Data in pair key and value
    scriptUtilities.readNSaveTestConfig()
    
    sCfg = ConfigParser.ConfigParser()
    sCfg.read(scriptGlobals.ScriptConfigFilePath )
    
    LoopNumber = General.clsGlobal.LoopitIteration
    DelayOffTimeArray = sCfg.get('POWER_PARAM', 'RF_OFF_TIME')
    DelayOnTimeArray = sCfg.get('POWER_PARAM', 'RF_ON_TIME')
    
    # Convert string to array
    
    DelayOffTimeArray = eval('['+DelayOffTimeArray+']')
    DelayOnTimeArray = eval('['+DelayOnTimeArray+']')
    
    testContinue = True;
    
    paramsLenArray2 = [len(DelayOffTimeArray), len(DelayOnTimeArray)]
    if (scriptUtilities.areAllEqual(paramsLenArray2) == False):
	print ("Error: Length of On/OFF parameters NOT equal ")
	scriptGlobals.TestAborted = True
    else:
	scriptGlobals.TestSignalLevelsList = Utilities.HelperFunctions.ParseTestLevels(testLevelStr) 
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
	if (testContinue == True):
	    scriptGlobals.MainFrame.Delay(5)
	    # set to high level for factory reset
	    if (scriptGlobals.SignalType.ToLower() == "dbhz"):
		defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,40,scriptGlobals.SignalType)
	    elif (scriptGlobals.SignalType.ToLower() == "dbm"):
		defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,-130,scriptGlobals.SignalType)
	    else:
		print "Signal Type is not correct"
		defaultAtten = 0
	    if (scriptGlobals.AttenSource == General.clsGlobal.SIM):
		defaultAtten = -scriptGlobals.SimInitialAtten;
	    # set atten 
	    scriptUtilities.setAtten(defaultAtten)    
	    
	    # setup each active com ports
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		
		# reset logfile name
		myPort.comm.Log.SetDurationLoggingStatusLabel("")
		if (myPort.comm.IsSourceDeviceOpen() == False):
		    if (myPort.comm.OpenPort() == False):
			portList[comIdx] = -1
			continue
		    myPort.RunAsyncProcess()
		if (myPort.SignalViewLocation.IsOpen == False):
		    mainFrame.CreateSignalViewWin(myPort)
		if (myPort.DebugViewLocation.IsOpen == False):
		    mainFrame.CreateDebugViewWin(myPort)
		myPort.comm.RxCtrl.OpenChannel("SSB")
		myPort.comm.RxCtrl.PollSWVersion()
		myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)

		myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)  
	
		comIdx = comIdx + 1
	    
	    mainFrame.UpdateGUIFromScript()
	    mainFrame.Delay(10)
	    # setup each active com ports
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		myPort.comm.RxCtrl.OpenChannel("SSB")
		myPort.comm.RxCtrl.OpenChannel("STAT")
	
		comId = comIdx + 1
	    
	    # Loop for each test levels    
	    for levelIndex in range(0, len(scriptGlobals.TestSignalLevelsList)):
		level = scriptGlobals.TestSignalLevelsList[levelIndex]
		if (scriptGlobals.TestAborted == True):
			print "Test aborted"
			break
		atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
		diffAtten = atten - defaultAtten
		if ((levelIndex == 0) and (diffAtten > 5)):
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

				    
	    
		# setup each active com ports
		comIdx = 0	
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    myPort.comm.RxCtrl.OpenChannel("SSB")
		    myPort.comm.RxCtrl.OpenChannel("STAT")
		    myPort.comm.RxCtrl.PollSWVersion()
		    # Create directory for log files
		    Now = time.localtime(time.time())
		    timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
		    testName = scriptGlobals.ScriptName
		    portLogFile = "%s%s_%s_%s_%s%s" %(scriptGlobals.TestResultsDirectory,testName,scriptGlobals.DutNamesList[comIdx],timeNowStr,myPort.comm.PortName,scriptGlobals.LogFileExtsList[comIdx])

		    # update Test Info	
		    if (level < 0):
			testSetupLevelString = "%s%s%s" %("m",`-level`.replace('.','p'),scriptGlobals.SignalType) 	
		    else:
			testSetupLevelString = "%s%s" %(`level`.replace('.','p'),scriptGlobals.SignalType) 
			
		    scriptGlobals.TestID = "%s-%s" % (scriptGlobals.TestName,testSetupLevelString)

		    Now = time.localtime(time.time())
		    scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
		    scriptUtilities.updateDUTInfo(comIdx)
		    myPort.comm.m_TestSetup.Atten = atten
		    
		    myPort.comm.Log.OpenFile(portLogFile)	
		    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Navlib_Data = True
		    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Development_Data = True
		    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableFullSystemReset = True
		    myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableEncryptedData = False
	
		    myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)
	
		    # Send full power mode
		    myPort.comm.LowPowerParams.Mode = 0;
		    myPort.comm.RxCtrl.SetPowerMode(False);
	
		    comIdx = comIdx + 1
	
		navStatus = False
		count = 0
		while((navStatus == False) and (count < 12)):
		    navStatus = mainFrame.GetNavStatus("*")
		    count = count + 2
		    print "Wait for nav loop %d" %(count)
		    if ((navStatus == True) or (count >= 12)):
			break
		    # if ((General.clsGlobal.Abort == True) or (General.clsGlobal.AbortSingle == True)):
			# scriptGlobals.TestAborted = True
			# break
		    mainFrame.Delay(3)
				
		for loopCnt in range(0, LoopNumber):
		    logStr = "Trials %d/%d"% (loopCnt+1,LoopNumber)
		    scriptUtilities.logApp("*",logStr)
		    for delayIndex in range(0,len(DelayOffTimeArray)):
			offT = DelayOffTimeArray[delayIndex]
			onT = DelayOnTimeArray[delayIndex]
			# turn off 
			scriptUtilities.rfOnOff(False,atten)
			mainFrame.Delay(5)
			#reset
			scriptUtilities.reset(resetType)
			# wait for off time
			logStr = "RF for %d seconds" % (offT)
			scriptUtilities.logApp("*",logStr)
			mainFrame.Delay(offT)
			# turn on RF
			# wait for on time
			scriptUtilities.rfOnOff(True,atten)
			logStr = "RF on for %d seconds" % (onT)
			scriptUtilities.logApp("*",logStr)
			mainFrame.Delay(onT)
	
		#cleanup
    
		comIdx = 0	
		for usePort in scriptGlobals.DutMainPortsList:	
		    if (usePort < 0):
			comIdx = comIdx + 1
			continue
		    myPort = scriptGlobals.DutPortManagersList[comIdx]
		    # Send full power mode
		    myPort.comm.LowPowerParams.Mode = 0;
		    myPort.comm.RxCtrl.SetPowerMode(False);
		    myPort.comm.Log.CloseFile()		    
		    comIdx = comIdx + 1 	        
		
	    # set default atten
	    scriptUtilities.setAtten(defaultAtten)	    
	    print "Done: %s" % (scriptGlobals.ScriptName)    
    
    mainFrame.SetScriptDone(True)
    
    #cleanup
    scriptGlobals.Exiting  = True
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# Send full power mode
	myPort.comm.ListenersCtrl.Stop()
	myPort.comm.ListenersCtrl.Cleanup()
	myPort.comm.ClosePort()
	# myPort.StopAsyncProcess()
	comIdx = comIdx + 1

except:
    scriptUtilities.ExceptionHandler()

