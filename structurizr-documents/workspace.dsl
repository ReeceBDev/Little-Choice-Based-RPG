workspace "Name" "Description" {

    !identifiers hierarchical

    model {
        user = person "User" "Represents a player. \n Connects to the game and interacts with a personal character."
	
		mrpg = softwareSystem "Multiplayer RPG" {
			description "The main game program. \n This is hosted on a server, which multiple users may access."
			tags mrpgTag
			
			webInterface = container "Web Interface" {
				description "Constructs a user interface from game-information."
				tags mrpgTag, webInterfaceTag
				
				input = component "User Input Box" {
					tags mrpgTag
				}
				output = component "Game Output" {
					tags mrpgTag
				}
			}
			
			group Little_Choice-Based_RPG {
				gameResource = container "Game-World Generator" {
					description "Generates the initial Game World"
					tags mrpgTag
					
					#Resources
						group Resources {
							gameObject = component "Game Objects" "Entities within the world. \n Includes every physical object, from furniture to enemies and even player's own characters." tags mrpgTag
							room = component "Rooms" "Descriptive location in the world, which the player can explore. \n Contains a list of Game Objects." tags mrpgTag
							choice = component "Choices" "Lets players choose to interact with objects. \n Each choice pipes a delegate on the object to players." tags mrpgTag
						}

						objectJSON = component "Entity List" {
							technology "JSON Objects"
							description "A list of entities that can get added to the world. \n For example, decoration, equippables, enemies."
							tags mrpgTag
						}
						roomJSON = component "Room List" {
							technology "JSON Objects"
							description "List of rooms that can get added to the world."
							tags mrpgTag
						}
					
						gameEnvironment = component "Game Environment" "Contains all the Rooms in a given location." tags mrpgTag
					
					#Internal Connections
						#Resources
							room -> gameEnvironment "Stored in"
							gameObject -> room "Stored in"
							choice -> gameObject "Stored in"
							
						#Generation
							objectJSON -> gameObject  "Provides blueprint for"
							roomJSON -> room "Provides blueprint for"

				}
				
				gameApp = container "Live Game Application" {
					description "Handles all game-logic."
					tags mrpgTag
					
					
					choiceHandler = component "Choice Handler" "Searches the room that a player is in for every possible choice." tags mrpgTag
					interfaceStyle = component "Interface Style" "Displays interface information in different ways." tags mrpgTag
					
					#Internal Connections
						interfaceStyle -> choiceHandler "Lists and executes choices"
				}
			}
			
			persistentStorage = container "Persistent Storage" {
				description "Records the entire game-state."
				tags mrpgTag, Database
				
				functions = component "Database Functions" {
					description "Gets and modifies the World-State Database. \n\n Maintains database health, such as providing conflict resolution functions."
					tags mrpgTag
				}
				
				permanence = component "Permanent World-State Database" {
					description "Stores all of the items, rooms, players, enemies and any other components that make up the world."
					tags "Database", mrpgTag
				}
				
				userTokenStore = component "User Token Store" {
					description "Associates playerdata reference with authentication tokens."
					tags "Database", mrpgTag
				}
				
				
				#Internal Connections
					#Database Functions
						functions -> permanence "Modifies"
						functions -> userTokenStore "Finds playerdata using token"
					
			}
			
			#Internal Connections
				#WebInterface Connections
					webInterface -> gameApp "Sends and receives commands"
					webInterface.input -> gameApp.interfaceStyle "Sends commands and retrieves output"					
					
				#Storage Connections
					gameResource.gameEnvironment -> persistentStorage.functions "Stored as world-state"
					gameApp.choiceHandler -> persistentStorage.functions "Reads and changes world-state"
		}
		
		userAAA = softwareSystem "Authentication, Authorisation and Accounting (AAA)" {
			description "Authenticates a user. Associates the user to a player profile. Logs all attemps and user interactions."
			tags aaaTag
			
			authentication = container "External Authentication" {
			technology "Google Authentication"
			description "Gives a user access to their account via the token returned. \n\n (No passwords or personal information need be stored locally.)"
			tags aaaTag
			}
			
			accounting = container "Logging" "Logs access, user inputs and any other metrics worth tracking." {
				tags "Database", aaaTag
			}
			
			#Internal Connections
				#Accounting
					authentication -> accounting "Logs authentication"
		}
		
		serverManager = softwareSystem "Server Manager" {
			description "Runs the game instance. \n Externalising this allows for scheduling, monitoring health and backups.
			tags serverManagerTag
			
			serverFunctions = container "Server Functions" "Monitors and keeps the server online and up-to-date." {
				tags serverManagerTag
			
				serverHandler = component "Server Handler" "Functionality to start, stop, restart and put into maintenance mode." tags serverManagerTag
				serverScheduler = component "Scheduler" "Schedules restarts, updates and backups." tags serverManagerTag
				healthMonitor = component "Health Monitor" "Monitors for heartbeat and internal latency." tags serverManagerTag
				updateMonitor = component "Update Monitor" "Monitors for git updates." tags serverManagerTag
				backupHandler = component "Backup Functions" "Handles backing up the World-State store." tags serverManagerTag
				backupStorage = component "Backup Database" {
					tags "Database", serverManagerTag
				}
				
				#Internal Connections
					#serverHandler
						updateMonitor -> serverScheduler "Notifies"
						healthMonitor -> serverScheduler "Makes emergency requests"
						serverScheduler -> serverHandler "Makes requests"
						serverHandler -> backupHandler "Requests backup"
				
					#BackupFunctions
						backupHandler -> backupStorage "Writes"
			}
			
			versionHandler = container "Git Update Handler" {
				description "Compares current and updated versions. Applies changes to the existing database. \n\n Will overwrite where necessary. \n Monitors for issues and arbiters resolutions."
				tags serverManagerTag
			}
			
			#Internal Connections
				#ServerFunctions
					serverFunctions.serverHandler -> versionHandler "Requests updates"
		}
		
		
		#System Context Connections
			user -> mrpg.webInterface "[Secure] Interacts"		
			user -> userAAA "Connects"
			userAAA -> mrpg "Logs In"
			mrpg -> serverManager "Sends Heartbeat"
			
			#AAA Container Connections
				userAAA.authentication -> mrpg.webInterface "Transmits Token"
				mrpg.webInterface -> userAAA.accounting "Records input to server"
			
			#Server Manager Container Connections
				serverManager.serverFunctions.serverHandler -> mrpg.gameApp "Starts, Stops and Monitors"
				serverManager.versionHandler -> mrpg.persistentStorage.functions "Merges updates"
				
				#Server Functions Component connections
					servermanager.serverFunctions.backupHandler -> mrpg.persistentStorage.permanence "Reads"
    }


    views {
		#Main View
			systemContext mrpg "LittleChoiceBasedRPG" {
				include *
				exclude mrpg->userAAA
				autolayout lr
			}
		
		#MultiplayerRPG
			container mrpg "MultiplayerRPG" {
				include *
				exclude serverManager userAAA
				exclude "mrpg.gameApp -> mrpg.webInterface"
				autolayout lr
			}
			
			#persistentStorage 
				component mrpg.persistentStorage {
					include *
					exclude "serverManager -> mrpg.gameApp"
					exclude "serverManager -> mrpg.persistentStorage.permanence"
					autolayout lr
				}
							
			#worldGenerator
				component mrpg.gameResource {
					include *
					autolayout lr
				}
			
			#liveGameApp
				component mrpg.gameApp {
					include *
					include user
					autolayout lr
				}
		
		#AAA
			container userAAA "AAA" {
			include *
			autolayout lr
			}
			
		#ServerManager
			container serverManager "ServerManager" {
				include *
				exclude mrpg
				include mrpg.gameApp mrpg.persistentStorage
				exclude "serverManager.serverFunctions -> mrpg.persistentStorage"
				autolayout lr
			}
			
			#serverFunctions
				component serverManager.serverFunctions "ServerFunctions" {
					include *
					exclude mrpg
					include mrpg.persistentStorage.permanence
					autolayout lr
				}
		
        styles {
			#Global
				element "Element" {
					color #121420
					background #E8E8E8
				}
				
				element "Person" {
					background #FFF7F4
					shape person
				}

				element "Database" {
					shape cylinder
				}

				element "webInterfaceTag" {
					shape WebBrowser
				}
			
			#System Context
				element "mrpgTag" {
					background #97CFD8
				}
				
				element "aaaTag" {
					background #ffa69e
				}
				
				element "serverManagerTag" {
					background #faf3dd
				}
			}
    }

    configuration {
        scope none
    }

}