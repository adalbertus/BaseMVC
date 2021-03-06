﻿Feature: New project creation
    In order to manage projects
    As a project manager
    I want to be able to create new project

Scenario: Open New Project page as Project Manager
    Given I am Project Manager
    When I open New Project page
    Then New Project page will be filled with default values

Scenario: After saving New Project we are redirected to Project List page
    Given I opened New Project page
    And I am Project Manager
    And I filled New Project page as follows
        | Field     | Value          |
        | Name      | Sample project |
        | StartDate | 2011-07-01     |
    When I press Save button
    Then I will be redirected to Project List page

Scenario: Saving New Project stores it in database
    Given I opened New Project page
    And I am Project Manager
    And I filled New Project page as follows
        | Field     | Value          |
        | Name      | Sample project |
        | StartDate | 2011-07-01     |
    When I press Save button
    Then project will be saved in database