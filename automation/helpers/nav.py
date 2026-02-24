import time


def _click_nav(window, title):
    window.child_window(title=title, control_type="Button").click()
    time.sleep(0.4)


def go_dashboard(window):
    _click_nav(window, "Dashboard")


def go_accounts(window):
    _click_nav(window, "Accounts")


def go_transactions(window):
    _click_nav(window, "Transactions")


def go_transfer(window):
    _click_nav(window, "Transfer")


def go_logout(window):
    _click_nav(window, "Logout")
