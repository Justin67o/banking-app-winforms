# import time
# import pytest
# from helpers.nav import go_transfer, go_dashboard


# @pytest.fixture(autouse=True)
# def navigate_to_transfer(main_window):
#     """Always start each test on the Transfer view."""
#     go_transfer(main_window)
#     yield main_window


# def _get_combos(window):
#     """Return the From and To ComboBoxes."""
#     combos = window.children(class_name="ComboBox")
#     return combos[0], combos[1]  # From, To


# def test_submit_with_no_selection_shows_error(main_window):
#     """Clicking Transfer with nothing filled in should show an error label."""
#     main_window.child_window(title="Transfer Money", control_type="Button").click()
#     time.sleep(0.3)

#     error = main_window.child_window(
#         title="Please fill in all fields correctly.", control_type="Text"
#     )
#     assert error.exists()


# def test_same_account_shows_error(main_window):
#     """Selecting the same account for From and To should show an error."""
#     combo_from, combo_to = _get_combos(main_window)

#     combo_from.select(0)
#     time.sleep(0.3)  # populates the To combo

#     combo_to.select(0)
#     main_window.child_window(class_name="NumericUpDown").set_edit_text("100")
#     main_window.child_window(title="Transfer Money", control_type="Button").click()
#     time.sleep(0.3)

#     error = main_window.child_window(
#         title="Cannot transfer to the same account.", control_type="Text"
#     )
#     assert error.exists()


# def test_zero_amount_shows_error(main_window):
#     """Leaving the amount at 0 should show a validation error."""
#     combo_from, combo_to = _get_combos(main_window)

#     combo_from.select(0)
#     time.sleep(0.3)
#     combo_to.select(0)

#     # Amount stays at 0 (default)
#     main_window.child_window(title="Transfer Money", control_type="Button").click()
#     time.sleep(0.3)

#     error = main_window.child_window(
#         title="Please fill in all fields correctly.", control_type="Text"
#     )
#     assert error.exists()


# def test_valid_transfer_shows_success(main_window):
#     """A valid transfer should show the success message."""
#     combo_from, combo_to = _get_combos(main_window)

#     combo_from.select(0)
#     time.sleep(0.3)  # populates To combo
#     combo_to.select(0)  # first available (different) account

#     main_window.child_window(class_name="NumericUpDown").set_edit_text("10.00")
#     main_window.child_window(class_name="TextBox").set_edit_text("Test transfer")

#     main_window.child_window(title="Transfer Money", control_type="Button").click()
#     time.sleep(0.5)

#     success = main_window.child_window(title_re=r".*Successfully transferred.*", control_type="Text")
#     assert success.exists()


# def test_successful_transfer_navigates_to_transactions(main_window):
#     """After a successful transfer the app should navigate to Transactions after 2 seconds."""
#     combo_from, combo_to = _get_combos(main_window)

#     combo_from.select(0)
#     time.sleep(0.3)
#     combo_to.select(0)

#     main_window.child_window(class_name="NumericUpDown").set_edit_text("10.00")
#     main_window.child_window(title="Transfer Money", control_type="Button").click()
#     time.sleep(3)  # wait for the 2-second timer in the app + buffer

#     # Transactions view has a ComboBox for filtering
#     assert main_window.child_window(class_name="ComboBox").exists()


# def test_cancel_resets_form(main_window):
#     """Clicking Cancel should clear all fields back to their defaults."""
#     combo_from, combo_to = _get_combos(main_window)

#     combo_from.select(0)
#     time.sleep(0.3)
#     combo_to.select(0)

#     main_window.child_window(class_name="NumericUpDown").set_edit_text("50.00")
#     main_window.child_window(class_name="TextBox").set_edit_text("Some description")

#     main_window.child_window(title="Cancel", control_type="Button").click()
#     time.sleep(0.3)

#     amount = main_window.child_window(class_name="NumericUpDown").window_text()
#     assert amount == "0.00" or amount == "0"
