#####################################################################################
#
# Copyright (c) SiRF Technology Inc. 
#
#####################################################################################

# import PCComm
import SiRFLive.Communication
import scriptGlobals
import scriptUtilities

try:
	scriptName = "portSetup"
	scriptGlobals.MainFrame.SetScriptDone(False)
	General.clsGlobal.ScriptError = False

	if (General.clsGlobal.IsMarketingUser()):		
		scriptGlobals.DutPortManagersList.append(mainFrame.GetFirstAvailablePort())
		if (scriptGlobals.DutPortManagersList[0] != 0):
			scriptGlobals.DutPortManagersList[0].comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.GPS
			scriptGlobals.DutPortManagersList[0].comm.Log.IsBin = False
			scriptGlobals.DutPortManagersList[0].comm.DisableDataPlot()
			scriptGlobals.DutMainPortsList = []
			scriptGlobals.DutMainPortsList.append(int(scriptGlobals.DutPortManagersList[0].comm.PortNum))
			# intialize flags to indicate RX get TTFF
			scriptGlobals.getTTFFs.append(False)
			scriptGlobals.getNavs.append(False)
			scriptGlobals.getTTBNavs.append(False)
			testPortList.append(1)
			# mainFrame.SetScriptDone(True)
		else:			
			print "Error running script: %s -- No comport detected" %(scriptName)
			# mainFrame.SetScriptDone(True)
			General.clsGlobal.ScriptError = True
	else:
		comIdx = 0
		for inPort in scriptGlobals.DutMainPortsList:
			if (inPort < 0):
				print "Port %d is invalid" % (inPort)	
				scriptGlobals.DutPortManagersList.append(mainFrame.CreateNewPort("COM9999"))
				testPortList.append(1)				
				comIdx = comIdx + 1
				continue
			myPhyConnection = scriptGlobals.DutSourceDeviceList[comIdx]
			if (myPhyConnection == "UART"):
				portNameString = "COM" + str(inPort)
			elif (myPhyConnection == "TCP/IP_CLIENT"):
				portNameString = "TCP" + str(inPort);
			elif (myPhyConnection == "TCP/IP_SERVER"):
				portNameString = "TCP" + str(inPort)			
			elif (myPhyConnection == "PLAYBACK_FILE"):
				portNameString = "PBK" + str(inPort)
			elif (myPhyConnection == "I2C"):
				portNameString = "I2C" + str(inPort)
			else:
				portNameString = str(inPort)
			
			scriptGlobals.DutPortManagersList.append(mainFrame.CreateNewPort(portNameString))
			# intialize flags to indicate RX get TTFF
			scriptGlobals.getTTFFs.append(False)
			scriptGlobals.getNavs.append(False)
			scriptGlobals.getTTBNavs.append(False)
			testPortList.append(1)
			# winComList[comIdx].Show()
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			myPort.comm.DisableDataPlot()
			myPort.comm.AutoDetectProtocolAndBaudDone = True
			myPort.comm.PortNum = str(inPort)
			myPort.comm.PortName = portNameString
			myPort.comm.BaudRate = str(scriptGlobals.DutBaudRateList[comIdx])
			myPort.comm.MessageProtocol = scriptGlobals.DutMessageProtocolsList[comIdx]
			myPort.comm.AidingProtocol = scriptGlobals.DutAidingProtocolsList[comIdx]
			# winComList[comIdx].comm.CUC.DisplayBuffer = 50
			myFamily = scriptGlobals.DutFamilyList[comIdx]
			if ( myFamily == "GSD4t"):
				myPort.comm.ProductFamily = CommonClass.ProductType.GSD4t
			elif (myFamily == "GSD4e"):
				myPort.comm.ProductFamily = CommonClass.ProductType.GSD4e
			elif (myFamily == "GSD4e"):
				myPort.comm.ProductFamily = CommonClass.ProductType.GSD4e			
			elif (myFamily == "SS3_GSW"):
				myPort.comm.ProductFamily = CommonClass.ProductType.GSW
			elif (myFamily == "SS3_SLC"):
				myPort.comm.ProductFamily = CommonClass.ProductType.sLC
			else:
				myPort.comm.ProductFamily = CommonClass.ProductType.GSD4t

			myType = scriptGlobals.DutTypesList[comIdx]	
			if (myType == "SLC"):
				myPort.comm.RxType = SiRFLive.Communication.CommunicationManager.ReceiverType.SLC
			elif (myType == "GSW"):
				myPort.comm.RxType = SiRFLive.Communication.CommunicationManager.ReceiverType.GSW
			else:
				myPort.comm.RxType = SiRFLive.Communication.CommunicationManager.ReceiverType.GSW
			
			myRxLogType = scriptGlobals.DutRxLogTypesList[comIdx]
			if (myRxLogType == "GP2"):
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2
				myPort.comm.Log.IsBin = False
			elif (myRxLogType == "HEX"):
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.Hex;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2
				myPort.comm.Log.IsBin = False
			elif (myRxLogType == "TXT"):
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.Text;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.Text
				myPort.comm.Log.IsBin = False
			elif (myRxLogType == "SSB"):
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.SSB;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2
				myPort.comm.Log.IsBin = False
			elif (myRxLogType == "GPS"):
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.GPS;
				myPort.comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.GPS;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.GPS
				myPort.comm.Log.IsBin = False
			elif (myRxLogType == "BIN"):
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.GPS;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.BIN
				myPort.comm.Log.IsBin = True
			else:
				myPort.comm.RxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2;
				myPort.comm.LogFormat = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2
				myPort.comm.Log.IsBin = False

			myTxLogType = scriptGlobals.DutTxLogTypesList[comIdx]
			if (myTxLogType == "GP2"):
				myPort.comm.TxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.GP2;
			elif (myTxLogType == "HEX"):
				myPort.comm.TxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.Hex;
			elif (myTxLogType == "TXT"):
				myPort.comm.TxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.Text;
			elif (myTxLogType == "SSB"):
				myPort.comm.TxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.SSB;
			elif (myTxLogType == "GPS"):
				myPort.comm.TxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.GPS;
			else:
				myPort.comm.TxCurrentTransmissionType = SiRFLive.Communication.CommunicationManager.TransmissionType.Hex;		
		
			# w.comm.Log.OpenFile("LA001AAA.gps")
			# winComList[comIdx].rx.rxInit(winComList[comIdx].comm)
			# winComList[comIdx].rx.resetCmd.resetInit(winComList[comIdx].comm)

			if (myPhyConnection == "UART"):
				print "Script open:" + myPort.comm.PortName + " at baud " + myPort.comm.BaudRate + "..."
				myPort.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232
			elif (myPhyConnection == "TCP/IP_CLIENT"):
				myPort.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Client
				myPort.comm.CMC.HostAppClient.TCPClientHostName = scriptGlobals.DutIPAddrsList[comIdx]
				myPort.comm.CMC.HostAppClient.TCPClientPortNum = inPort
				print "Script open TCP port:" + `inPort` + " at " + scriptGlobals.DutIPAddrsList[comIdx]
			elif (myPhyConnection == "TCP/IP_SERVER"):
				myPort.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Server
			elif (myPhyConnection == "I2C"):
				myPort.comm.InputDeviceMode = CommonClass.InputDeviceModes.I2C
				myPort.comm.CMC.HostAppI2CSlave.I2CDevicePortNum = inPort
				myPort.comm.CMC.HostAppI2CSlave.I2CDevicePortNumMaster = inPort + 1
				myPort.comm.CMC.HostAppI2CSlave.I2CSlaveAddress = 98
				myPort.comm.CMC.HostAppI2CSlave.I2CMasterAddress = 96
				#myPort.comm.Flag_I2CAutoDetDone =  True
				print "Script open I2C port: (%d:%d:%d:%d)" % (myPort.comm.CMC.HostAppI2CSlave.I2CDevicePortNum,myPort.comm.CMC.HostAppI2CSlave.I2CDevicePortNumMaster,myPort.comm.CMC.HostAppI2CSlave.I2CSlaveAddress,myPort.comm.CMC.HostAppI2CSlave.I2CMasterAddress)
			elif (myPhyConnection == "PLAYBACK_FILE"):
				myPort.comm.InputDeviceMode = CommonClass.InputDeviceModes.PlayBack_File
			else:
				print "Script: Opening " + myPort.comm.PortName + " at baud " + myPort.comm.BaudRate + "..."
				myPort.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232
			
			# mainFrame.SetCommWinRef(winComList[comIdx].comm.PortName, winComList[comIdx])
			# winComList[comIdx].comm.OpenPort()
			# winComList[comIdx].UpdateGUIFromComm()
			# if (scriptGlobals.DutTTBPortsList[comIdx] > 0):
			#	myPort.comm.OpenTTBPort(scriptGlobals.DutTTBPortsList[comIdx])		
			
			comIdx = comIdx + 1
	
	# scriptGlobals.DutPortManagersList = winComList
	mainFrame.UpdateDataFromScript()
	print "Done DUT setup."
	mainFrame.SetScriptDone(True)
	
except:
	scriptError = sys.exc_info()
	if (General.clsGlobal.Abort == False):
		if (len(scriptError) >= 3):
			trackBack = scriptError[2]
			scriptErrorMessage = scriptError[1]
			lineno = trackBack.tb_lineno
			print "Error Message: %s -- Line no: %d" % (scriptErrorMessage,lineno)
		print "Error running script: %s" %(scriptGlobals.ScriptName)
	
	sys.exc_clear()
	scriptGlobals.MainFrame.SetScriptDone(True) 
	
