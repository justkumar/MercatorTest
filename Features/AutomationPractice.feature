Feature: Automation practice test
	Verify that user can add an item to cart 

Background: 
	Given the browser opens on the "http://automationpractice.com/index.php" home page

@regression
Scenario Outline: Verify that user can add highest price item to the cart
	# Choose an item
	When the user clicks Dress menu  
	And the user adds highest price item to cart 
	# Verify the cart
	Then the user see a confirmation message with Proceed to checkout button 