import time
import pytest
from helpers.nav import go_dashboard, go_accounts, go_transactions, go_transfer


@pytest.fixture(autouse=True)
def navigate_to_dashboard(main_window):
    """Always start each dashboard test on the Dashboard view."""
    go_dashboard(main_window)
    yield main_window


def test_stat_cards_visible(main_window):
    """The three summary stat cards should all be present."""
    assert main_window.child_window(title_re=r".*Total Balance.*", control_type="Text").exists()
    assert main_window.child_window(title_re=r".*Accounts.*", control_type="Text").exists()
    assert main_window.child_window(title_re=r".*Recent.*", control_type="Text").exists()


def test_transfer_button_navigates(main_window):
    """Clicking '💸 Transfer Money' should switch to the Transfer view."""
    main_window.child_window(title="💸 Transfer Money", control_type="Button").click()
    time.sleep(0.4)

    # Transfer view contains the 'Transfer Money' submit button
    assert main_window.child_window(title="Transfer Money", control_type="Button").exists()


def test_view_transactions_button_navigates(main_window):
    """Clicking '📋 View Transactions' should switch to the Transactions view."""
    go_dashboard(main_window)
    main_window.child_window(title="📋 View Transactions", control_type="Button").click()
    time.sleep(0.4)

    # Transactions view has a ComboBox for account filtering
    assert main_window.child_window(class_name="ComboBox").exists()


def test_view_all_accounts_link(main_window):
    """Clicking 'View All →' in the Accounts panel should navigate to Accounts."""
    go_dashboard(main_window)
    # There are two 'View All →' links; index 0 is the Accounts one
    main_window.child_window(title="View All →", control_type="Hyperlink", found_index=0).click()
    time.sleep(0.4)

    assert main_window.child_window(title="My Accounts", control_type="Text").exists()


def test_view_all_transactions_link(main_window):
    """Clicking 'View All →' in the Transactions panel should navigate to Transactions."""
    go_dashboard(main_window)
    # Index 1 is the Transactions 'View All →' link
    main_window.child_window(title="View All →", control_type="Hyperlink", found_index=1).click()
    time.sleep(0.4)

    assert main_window.child_window(class_name="ComboBox").exists()
