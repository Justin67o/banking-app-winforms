# import time
# import pytest
# from helpers.nav import go_accounts, go_dashboard


# @pytest.fixture(autouse=True)
# def navigate_to_accounts(main_window):
#     """Always start each test on the Accounts view."""
#     go_accounts(main_window)
#     yield main_window


# def test_account_cards_visible(main_window):
#     """At least one account card should be visible."""
#     # Account cards are Panels/Panes — check that the Total Balance bar is present
#     assert main_window.child_window(title="Total Balance", control_type="Text").exists()


# def test_click_account_card_opens_details(main_window):
#     """Clicking an account card should navigate to Account Details."""
#     # Account cards are the first set of Pane controls in the accounts flow
#     # found_index=0 picks the first account card
#     main_window.child_window(control_type="Pane", found_index=0).click()
#     time.sleep(0.4)

#     # Account Details has a Back button
#     assert main_window.child_window(title="← Back to Accounts", control_type="Button").exists()


# def test_back_button_returns_to_accounts(main_window):
#     """Clicking Back from Account Details should return to Accounts."""
#     main_window.child_window(control_type="Pane", found_index=0).click()
#     time.sleep(0.4)

#     main_window.child_window(title="← Back to Accounts", control_type="Button").click()
#     time.sleep(0.4)

#     assert main_window.child_window(title="Total Balance", control_type="Text").exists()


# def test_account_details_transfer_button(main_window):
#     """Transfer Money quick action in Account Details should navigate to Transfer."""
#     main_window.child_window(control_type="Pane", found_index=0).click()
#     time.sleep(0.4)

#     main_window.child_window(title="💸\r\nTransfer Money", control_type="Button").click()
#     time.sleep(0.4)

#     assert main_window.child_window(title="Transfer Money", control_type="Button").exists()


# def test_account_details_view_transactions_button(main_window):
#     """View All Transactions quick action should navigate to Transactions."""
#     go_accounts(main_window)
#     main_window.child_window(control_type="Pane", found_index=0).click()
#     time.sleep(0.4)

#     main_window.child_window(title="📋\r\nView All Transactions", control_type="Button").click()
#     time.sleep(0.4)

#     assert main_window.child_window(class_name="ComboBox").exists()
