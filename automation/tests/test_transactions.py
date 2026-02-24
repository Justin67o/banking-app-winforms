import time
import pytest
from helpers.nav import go_transactions


@pytest.fixture(autouse=True)
def navigate_to_transactions(main_window):
    """Always start each test on the Transactions view."""
    go_transactions(main_window)
    yield main_window


def test_transaction_rows_visible(main_window):
    """With 'All Accounts' selected there should be at least one transaction row."""
    combo = main_window.child_window(class_name="ComboBox")
    combo.select("All Accounts")
    time.sleep(0.3)

    # Transaction rows are Panes inside the transactions panel
    rows = main_window.children(control_type="Pane")
    assert len(rows) > 0, "Expected at least one transaction row"


def test_filter_by_account(main_window):
    """Selecting a specific account in the ComboBox should filter the list."""
    combo = main_window.child_window(class_name="ComboBox")

    # Select 'All Accounts' first and count rows
    combo.select("All Accounts")
    time.sleep(0.3)
    all_rows = len(main_window.children(control_type="Pane"))

    # Select the first real account (index 1 in the combo)
    combo.select(1)
    time.sleep(0.3)
    filtered_rows = len(main_window.children(control_type="Pane"))

    # Filtered list should be <= the full list
    assert filtered_rows <= all_rows


def test_reset_filter_to_all_accounts(main_window):
    """Switching back to 'All Accounts' after filtering should restore the full list."""
    combo = main_window.child_window(class_name="ComboBox")

    combo.select("All Accounts")
    time.sleep(0.3)
    all_rows = len(main_window.children(control_type="Pane"))

    combo.select(1)
    time.sleep(0.3)

    combo.select("All Accounts")
    time.sleep(0.3)
    restored_rows = len(main_window.children(control_type="Pane"))

    assert restored_rows == all_rows
